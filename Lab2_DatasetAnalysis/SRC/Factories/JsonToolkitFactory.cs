using System.Text.Json;
using DataLensPatterns.Core.Models;

namespace Lab2_AbstractFactoryPrototype.Factories;

public sealed class JsonToolkitFactory : IAnalysisToolkitFactory
{
    public IDataSetLoader CreateLoader() => new JsonLoader();
    public IAnalysisExporter CreateExporter() => new JsonAnalysisExporter();

    private sealed class JsonLoader : IDataSetLoader
    {
        public TabularDataset Load(string path)
        {
            using var document = JsonDocument.Parse(File.ReadAllText(path));
            if (document.RootElement.ValueKind != JsonValueKind.Array)
                throw new FormatException("Ожидается JSON-массив объектов.");
            var elements = document.RootElement.EnumerateArray().ToList();
            if (elements.Count == 0) throw new FormatException("JSON-массив не содержит записей.");
            var columns = elements[0].EnumerateObject().Select(p => p.Name).ToList();
            var rows = new List<IReadOnlyDictionary<string, string?>>();
            foreach (var element in elements)
            {
                var row = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
                foreach (var column in columns)
                {
                    if (!element.TryGetProperty(column, out var value) || value.ValueKind == JsonValueKind.Null)
                        row[column] = null;
                    else row[column] = value.ValueKind == JsonValueKind.String ? value.GetString() : value.ToString();
                }
                rows.Add(row);
            }
            return new TabularDataset(Path.GetFileNameWithoutExtension(path), columns, rows);
        }
    }

    private sealed class JsonAnalysisExporter : IAnalysisExporter
    {
        public string Export(AnalysisReport report) => JsonSerializer.Serialize(new
        {
            report.DatasetName, report.RowCount, report.ColumnCount,
            report.MissingCellCount, report.CompletenessPercent, report.Columns
        }, new JsonSerializerOptions { WriteIndented = true });
    }
}
