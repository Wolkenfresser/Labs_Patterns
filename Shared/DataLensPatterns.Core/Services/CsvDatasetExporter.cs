using System.Globalization;
using DataLensPatterns.Core.Models;

namespace DataLensPatterns.Core.Services;

public sealed class CsvDatasetExporter
{
    public string Export(TabularDataset dataset)
    {
        var lines = new List<string> { string.Join(",", dataset.Columns.Select(Escape)) };
        foreach (var row in dataset.Rows)
            lines.Add(string.Join(",", dataset.Columns.Select(column => Escape(row.TryGetValue(column, out var value) ? value ?? "" : ""))));
        return string.Join(Environment.NewLine, lines);
    }

    public string Export(AnalysisReport report)
    {
        var lines = new List<string> { "column,kind,values,missing,min,max,average,median" };
        foreach (var column in report.Columns)
        {
            lines.Add(string.Join(",", new[]
            {
                Escape(column.Name), Escape(column.Kind.ToString()), column.ValueCount.ToString(CultureInfo.InvariantCulture),
                column.MissingCount.ToString(CultureInfo.InvariantCulture),
                Number(column.Minimum), Number(column.Maximum), Number(column.Average), Number(column.Median)
            }));
        }
        return string.Join(Environment.NewLine, lines);
    }

    private static string Number(double? value) => value?.ToString("0.####", CultureInfo.InvariantCulture) ?? "";

    private static string Escape(string value)
    {
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n') || value.Contains('\r'))
            return "\"" + value.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
        return value;
    }
}
