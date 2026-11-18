using DataLensPatterns.Core.Models;

namespace Lab5_FacadeFlyweight.Flyweight;

/// <summary>Intrinsic-state объект стиля общий для всех столбцов одного типа.</summary>
public sealed record ColumnStyle(ColumnDataKind Kind, string Symbol, string Description);

public sealed class ColumnStyleFactory
{
    private readonly Dictionary<ColumnDataKind, ColumnStyle> _cache = new();
    private readonly object _sync = new();
    public int CreatedCount { get { lock (_sync) return _cache.Count; } }

    public ColumnStyle GetStyle(ColumnDataKind kind)
    {
        lock (_sync)
        {
            if (_cache.TryGetValue(kind, out var existing)) return existing;
            var style = kind switch
            {
                ColumnDataKind.Numeric => new ColumnStyle(kind, "#", "Числовой столбец"),
                ColumnDataKind.Text => new ColumnStyle(kind, "Aa", "Текстовый столбец"),
                ColumnDataKind.Mixed => new ColumnStyle(kind, "A#", "Смешанные значения"),
                _ => new ColumnStyle(kind, "∅", "Пустой столбец")
            };
            _cache[kind] = style;
            return style;
        }
    }
}

public sealed class ColumnStyleRenderer(ColumnStyleFactory factory)
{
    public void Render(AnalysisReport report)
    {
        foreach (var column in report.Columns)
        {
            var style = factory.GetStyle(column.Kind);
            Console.WriteLine($"{style.Symbol,-3} {column.Name,-12} {style.Description,-24} пропусков: {column.MissingCount}");
        }
    }
}
