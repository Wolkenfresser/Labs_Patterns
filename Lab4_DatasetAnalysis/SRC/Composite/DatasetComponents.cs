using DataLensPatterns.Core.Models;

namespace Lab4_CompositeDecorator.Composite;

public interface IDatasetComponent
{
    string Name { get; }
    TabularDataset GetDataset();
}

public sealed class DatasetLeaf(string name, TabularDataset dataset) : IDatasetComponent
{
    public string Name { get; } = name;
    public TabularDataset GetDataset() => dataset;
}

/// <summary>Компоновщик объединяет листья и другие группы в древовидную структуру.</summary>
public sealed class DatasetGroup(string name) : IDatasetComponent
{
    private readonly List<IDatasetComponent> _children = new();
    public string Name { get; } = name;
    public IReadOnlyList<IDatasetComponent> Children => _children;
    public void Add(IDatasetComponent component) => _children.Add(component ?? throw new ArgumentNullException(nameof(component)));
    public bool Remove(IDatasetComponent component) => _children.Remove(component);

    public TabularDataset GetDataset()
    {
        if (_children.Count == 0) throw new InvalidOperationException($"Группа '{Name}' пуста.");
        return TabularDataset.Merge(Name, _children.Select(child => child.GetDataset()).ToList());
    }
}
