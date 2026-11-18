using System.Text;
using DataLensPatterns.Core.Models;

namespace Lab4_CompositeDecorator.Decorators;

public interface IAnalysisReportFormatter { string Format(AnalysisReport report); }

public sealed class BasicReportFormatter : IAnalysisReportFormatter
{
    public string Format(AnalysisReport report) =>
        $"Набор: {report.DatasetName}; строк: {report.RowCount}; столбцов: {report.ColumnCount}; " +
        $"пропусков: {report.MissingCellCount}; заполненность: {report.CompletenessPercent:F2}%";
}

public abstract class ReportFormatterDecorator(IAnalysisReportFormatter inner) : IAnalysisReportFormatter
{
    protected IAnalysisReportFormatter Inner { get; } = inner;
    public abstract string Format(AnalysisReport report);
}

public sealed class MissingValuesDecorator(IAnalysisReportFormatter inner) : ReportFormatterDecorator(inner)
{
    public override string Format(AnalysisReport report)
    {
        var builder = new StringBuilder(Inner.Format(report));
        builder.AppendLine("\nПропуски по столбцам:");
        foreach (var c in report.Columns.Where(c => c.MissingCount > 0))
            builder.AppendLine($"- {c.Name}: {c.MissingCount}");
        return builder.ToString();
    }
}

public sealed class NumericStatisticsDecorator(IAnalysisReportFormatter inner) : ReportFormatterDecorator(inner)
{
    public override string Format(AnalysisReport report)
    {
        var builder = new StringBuilder(Inner.Format(report));
        builder.AppendLine("\nЧисловые показатели:");
        foreach (var c in report.Columns.Where(c => c.Kind == ColumnDataKind.Numeric))
            builder.AppendLine($"- {c.Name}: min={c.Minimum:F2}; max={c.Maximum:F2}; среднее={c.Average:F2}; медиана={c.Median:F2}");
        return builder.ToString();
    }
}
