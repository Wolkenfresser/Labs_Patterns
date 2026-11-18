using System.Text;
using DataLensPatterns.Core.Models;

namespace DataLensPatterns.Core.Services;

/// <summary>Минимальный CSV-загрузчик с поддержкой разделителей , и ;, кавычек и экранированных кавычек.</summary>
public sealed class CsvDatasetReader
{
    public TabularDataset Load(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Не указан путь к CSV-файлу.", nameof(filePath));
        if (!File.Exists(filePath))
            throw new FileNotFoundException("CSV-файл не найден.", filePath);
        var text = File.ReadAllText(filePath, Encoding.UTF8);
        return Parse(text, Path.GetFileNameWithoutExtension(filePath));
    }

    public TabularDataset Parse(string content, string name = "dataset")
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new FormatException("CSV-файл пуст.");

        var lines = content.Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n').Split('\n');
        var headerLineIndex = Array.FindIndex(lines, line => !string.IsNullOrWhiteSpace(line));
        if (headerLineIndex < 0)
            throw new FormatException("CSV-файл не содержит заголовка.");

        var delimiter = CountOutsideQuotes(lines[headerLineIndex], ';') >
                        CountOutsideQuotes(lines[headerLineIndex], ',') ? ';' : ',';
        var headers = ParseLine(lines[headerLineIndex], delimiter)
            .Select((value, index) => index == 0 ? value.Trim().TrimStart('\uFEFF') : value.Trim())
            .ToList();
        if (headers.Count == 0 || headers.Any(string.IsNullOrWhiteSpace))
            throw new FormatException("Каждый столбец должен иметь непустое имя.");
        if (headers.Distinct(StringComparer.OrdinalIgnoreCase).Count() != headers.Count)
            throw new FormatException("В CSV обнаружены повторяющиеся имена столбцов.");

        var rows = new List<IReadOnlyDictionary<string, string?>>();
        for (var lineIndex = headerLineIndex + 1; lineIndex < lines.Length; lineIndex++)
        {
            if (string.IsNullOrWhiteSpace(lines[lineIndex]))
                continue;
            var values = ParseLine(lines[lineIndex], delimiter);
            if (values.Count != headers.Count)
                throw new FormatException($"Строка {lineIndex + 1}: ожидалось {headers.Count} полей, получено {values.Count}.");
            var row = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
            for (var columnIndex = 0; columnIndex < headers.Count; columnIndex++)
                row[headers[columnIndex]] = string.IsNullOrWhiteSpace(values[columnIndex]) ? null : values[columnIndex].Trim();
            rows.Add(row);
        }
        return new TabularDataset(name, headers, rows);
    }

    private static int CountOutsideQuotes(string line, char separator)
    {
        var count = 0;
        var quoted = false;
        for (var i = 0; i < line.Length; i++)
        {
            if (line[i] == '"')
            {
                if (quoted && i + 1 < line.Length && line[i + 1] == '"') { i++; continue; }
                quoted = !quoted;
            }
            else if (!quoted && line[i] == separator) count++;
        }
        return count;
    }

    private static List<string> ParseLine(string line, char delimiter)
    {
        var fields = new List<string>();
        var current = new StringBuilder();
        var inQuotes = false;
        for (var i = 0; i < line.Length; i++)
        {
            var ch = line[i];
            if (ch == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current.Append('"');
                    i++;
                }
                else inQuotes = !inQuotes;
            }
            else if (ch == delimiter && !inQuotes)
            {
                fields.Add(current.ToString());
                current.Clear();
            }
            else current.Append(ch);
        }
        if (inQuotes)
            throw new FormatException("Обнаружена незакрытая кавычка в CSV-строке.");
        fields.Add(current.ToString());
        return fields;
    }
}
