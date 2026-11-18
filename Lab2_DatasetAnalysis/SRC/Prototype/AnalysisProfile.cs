using DataLensPatterns.Core.Models;

namespace Lab2_AbstractFactoryPrototype.Prototype;

public sealed class AnalysisProfile
{
    public string Name { get; }
    public List<string> SelectedColumns { get; }
    public NumericFilter? Filter { get; }

    public AnalysisProfile(string name, IEnumerable<string> selectedColumns, NumericFilter? filter)
    {
        Name = name;
        SelectedColumns = selectedColumns.ToList();
        Filter = filter;
    }

    public AnalysisProfile Clone() => new(Name + " (копия)", SelectedColumns.ToList(),
        Filter is null ? null : new NumericFilter(Filter.ColumnName, Filter.Minimum, Filter.Maximum));
}

public sealed class AnalysisProfileRegistry
{
    private readonly Dictionary<string, AnalysisProfile> _prototypes = new(StringComparer.OrdinalIgnoreCase);
    public void Register(string key, AnalysisProfile prototype) => _prototypes[key] = prototype.Clone();
    public AnalysisProfile Create(string key)
    {
        if (!_prototypes.TryGetValue(key, out var prototype))
            throw new KeyNotFoundException($"Прототип профиля '{key}' не найден.");
        return prototype.Clone();
    }
}
