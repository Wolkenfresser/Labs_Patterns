using System.Text;
using DataLensPatterns.Core.Models;

namespace Lab3_AdapterBridge.Bridge;

public interface IReportRenderer
{
    string Render(AnalysisReport report, bool detailed);
}

public sealed class PlainTextReportRenderer : IReportRenderer
{
    public string Render(AnalysisReport report, bool detailed)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"Набор: {report.DatasetName}; строк: {report.RowCount}; столбцов: {report.ColumnCount}");
        builder.AppendLine($"Пропусков: {report.MissingCellCount}; заполненность: {report.CompletenessPercent:F2}%");
        if (detailed)
            foreach (var column in report.Columns)
                builder.AppendLine($"- {column.Name}: {column.Kind}; значений={column.ValueCount}; пропусков={column.MissingCount}; среднее={column.Average?.ToString("F2") ?? "н/д"}");
        return builder.ToString();
    }
}

public sealed class MarkdownReportRenderer : IReportRenderer
{
    public string Render(AnalysisReport report, bool detailed)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"## Анализ набора `{report.DatasetName}`");
        builder.AppendLine($"- Строк: **{report.RowCount}**; столбцов: **{report.ColumnCount}**");
        builder.AppendLine($"- Пропущенных ячеек: **{report.MissingCellCount}**; заполненность: **{report.CompletenessPercent:F2}%**");
        if (detailed)
        {
            builder.AppendLine("\n| Столбец | Тип | Пропуски | Среднее | Медиана |");
            builder.AppendLine("|---|---|---:|---:|---:|");
            foreach (var c in report.Columns)
                builder.AppendLine($"| {c.Name} | {c.Kind} | {c.MissingCount} | {c.Average?.ToString("F2") ?? "—"} | {c.Median?.ToString("F2") ?? "—"} |");
        }
        return builder.ToString();
    }
}

/// <summary>Абстракция Bridge отделяет уровень детализации представления от формата рендерера.</summary>
public abstract class AnalysisReportView(IReportRenderer renderer)
{
    protected IReportRenderer Renderer { get; } = renderer;
    protected abstract bool Detailed { get; }
    public string Show(AnalysisReport report) => Renderer.Render(report, Detailed);
}

public sealed class SummaryReportView(IReportRenderer renderer) : AnalysisReportView(renderer)
{
    protected override bool Detailed => false;
}

public sealed class DetailedReportView(IReportRenderer renderer) : AnalysisReportView(renderer)
{
    protected override bool Detailed => true;
}
