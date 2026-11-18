using DataLensPatterns.Core.Models;

namespace Lab3_AdapterBridge.Adapter;

/// <summary>Адаптер переводит legacy-формат string[][] в общую модель TabularDataset.</summary>
public sealed class LegacyDatasetAdapter(ILegacyTableProvider legacy) : IDatasetSource
{
    public TabularDataset Load()
    {
        var matrix = legacy.GetRows();
        if (matrix.Length < 2) throw new FormatException("Старый источник должен содержать заголовок и хотя бы одну строку.");
        var headers = matrix[0].Select(value => value.Trim()).ToArray();
        var rows = new List<IReadOnlyDictionary<string, string?>>();
        for (var i = 1; i < matrix.Length; i++)
        {
            if (matrix[i].Length != headers.Length)
                throw new FormatException($"Legacy-строка {i + 1} имеет неверное число полей.");
            var row = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
            for (var j = 0; j < headers.Length; j++) row[headers[j]] = string.IsNullOrWhiteSpace(matrix[i][j]) ? null : matrix[i][j];
            rows.Add(row);
        }
        return new TabularDataset("legacy_sales", headers, rows);
    }
}
