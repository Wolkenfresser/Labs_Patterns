using System.Globalization;
using DataLensPatterns.Core.Models;

namespace DataLensPatterns.Core.Services;

public sealed class DatasetAnalyzer
{
    public AnalysisReport Analyze(TabularDataset dataset)
    {
        ArgumentNullException.ThrowIfNull(dataset);
        var summaries = new List<ColumnSummary>();
        var missingTotal = 0;
        foreach (var column in dataset.Columns)
        {
            var values = dataset.Rows.Select(row => row.TryGetValue(column, out var value) ? value : null).ToList();
            var nonMissing = values.Where(value => !string.IsNullOrWhiteSpace(value)).ToList();
            var missing = values.Count - nonMissing.Count;
            missingTotal += missing;
            var numeric = new List<double>();
            var nonNumericCount = 0;
            foreach (var value in nonMissing)
            {
                if (double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands,
                        CultureInfo.InvariantCulture, out var number) && double.IsFinite(number))
                    numeric.Add(number);
                else nonNumericCount++;
            }

            var kind = nonMissing.Count == 0 ? ColumnDataKind.Empty
                : numeric.Count == 0 ? ColumnDataKind.Text
                : nonNumericCount == 0 ? ColumnDataKind.Numeric
                : ColumnDataKind.Mixed;
            numeric.Sort();
            var average = numeric.Count == 0 ? (double?)null : numeric.Average();
            var median = numeric.Count == 0 ? (double?)null
                : numeric.Count % 2 == 1 ? numeric[numeric.Count / 2]
                : (numeric[numeric.Count / 2 - 1] + numeric[numeric.Count / 2]) / 2.0;
            summaries.Add(new ColumnSummary(column, kind, nonMissing.Count, missing,
                numeric.Count == 0 ? null : numeric[0],
                numeric.Count == 0 ? null : numeric[^1], average, median));
        }

        var totalCells = dataset.RowCount() * dataset.Columns.Count;
        var completeness = totalCells == 0 ? 100.0 : 100.0 * (totalCells - missingTotal) / totalCells;
        return new AnalysisReport(dataset.Name, dataset.Rows.Count, dataset.Columns.Count,
            missingTotal, completeness, summaries);
    }

    public TabularDataset FilterNumeric(TabularDataset dataset, NumericFilter filter)
    {
        ArgumentNullException.ThrowIfNull(dataset);
        ArgumentNullException.ThrowIfNull(filter);
        if (!dataset.Columns.Contains(filter.ColumnName, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException($"Столбец '{filter.ColumnName}' не найден.", nameof(filter));
        if (!double.IsFinite(filter.Minimum) || !double.IsFinite(filter.Maximum) || filter.Minimum > filter.Maximum)
            throw new ArgumentException("Границы фильтра должны быть конечными числами, причём минимум не больше максимума.", nameof(filter));

        var filteredRows = dataset.Rows.Where(row =>
            row.TryGetValue(filter.ColumnName, out var raw) &&
            double.TryParse(raw, NumberStyles.Float | NumberStyles.AllowThousands,
                CultureInfo.InvariantCulture, out var value) &&
            double.IsFinite(value) && value >= filter.Minimum && value <= filter.Maximum);
        return new TabularDataset(dataset.Name + "_filtered", dataset.Columns, filteredRows);
    }
}

internal static class DatasetExtensions
{
    public static int RowCount(this TabularDataset dataset) => dataset.Rows.Count;
}
