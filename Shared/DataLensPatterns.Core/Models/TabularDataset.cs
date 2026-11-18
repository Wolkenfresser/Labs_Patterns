using System.Collections.ObjectModel;

namespace DataLensPatterns.Core.Models;

/// <summary>Неизменяемое представление табличного набора данных.</summary>
public sealed class TabularDataset
{
    public string Name { get; }
    public IReadOnlyList<string> Columns { get; }
    public IReadOnlyList<IReadOnlyDictionary<string, string?>> Rows { get; }

    public TabularDataset(
        string name,
        IEnumerable<string> columns,
        IEnumerable<IReadOnlyDictionary<string, string?>> rows)
    {
        Name = string.IsNullOrWhiteSpace(name) ? "dataset" : name.Trim();
        var columnList = columns.Select(c => c.Trim()).ToList();
        if (columnList.Count == 0)
            throw new ArgumentException("Набор данных должен содержать хотя бы один столбец.", nameof(columns));
        if (columnList.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("Имена столбцов не должны быть пустыми.", nameof(columns));
        if (columnList.Distinct(StringComparer.OrdinalIgnoreCase).Count() != columnList.Count)
            throw new ArgumentException("Имена столбцов должны быть уникальными.", nameof(columns));

        Columns = new ReadOnlyCollection<string>(columnList);
        var copiedRows = new List<IReadOnlyDictionary<string, string?>>();
        foreach (var sourceRow in rows)
        {
            var copy = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
            foreach (var column in columnList)
                copy[column] = sourceRow.TryGetValue(column, out var value) ? value : null;
            copiedRows.Add(new ReadOnlyDictionary<string, string?>(copy));
        }
        Rows = new ReadOnlyCollection<IReadOnlyDictionary<string, string?>>(copiedRows);
    }

    public TabularDataset TakeRows(int skip, int take, string? name = null)
    {
        if (skip < 0 || take < 0)
            throw new ArgumentOutOfRangeException(nameof(skip), "Индексы и количество строк не могут быть отрицательными.");
        return new TabularDataset(name ?? Name, Columns,
            Rows.Skip(skip).Take(take));
    }

    public static TabularDataset Merge(string name, IReadOnlyList<TabularDataset> datasets)
    {
        if (datasets.Count == 0)
            throw new ArgumentException("Для объединения необходим хотя бы один набор данных.", nameof(datasets));
        var expectedColumns = datasets[0].Columns;
        foreach (var dataset in datasets.Skip(1))
        {
            if (!expectedColumns.SequenceEqual(dataset.Columns, StringComparer.OrdinalIgnoreCase))
                throw new InvalidOperationException("Наборы данных можно объединять только при совпадающей структуре столбцов.");
        }
        return new TabularDataset(name, expectedColumns, datasets.SelectMany(d => d.Rows));
    }
}
