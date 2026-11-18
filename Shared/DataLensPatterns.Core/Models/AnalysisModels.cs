namespace DataLensPatterns.Core.Models;

public enum ColumnDataKind
{
    Empty,
    Numeric,
    Text,
    Mixed
}

public sealed record NumericFilter(string ColumnName, double Minimum, double Maximum);

public sealed record ColumnSummary(
    string Name,
    ColumnDataKind Kind,
    int ValueCount,
    int MissingCount,
    double? Minimum,
    double? Maximum,
    double? Average,
    double? Median);

public sealed class AnalysisReport
{
    public string DatasetName { get; }
    public int RowCount { get; }
    public int ColumnCount { get; }
    public int MissingCellCount { get; }
    public double CompletenessPercent { get; }
    public IReadOnlyList<ColumnSummary> Columns { get; }

    public AnalysisReport(string datasetName, int rowCount, int columnCount,
        int missingCellCount, double completenessPercent,
        IReadOnlyList<ColumnSummary> columns)
    {
        DatasetName = datasetName;
        RowCount = rowCount;
        ColumnCount = columnCount;
        MissingCellCount = missingCellCount;
        CompletenessPercent = completenessPercent;
        Columns = columns;
    }
}
