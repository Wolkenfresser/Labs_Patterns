using System.Globalization;
using DataLensPatterns.Core.Models;
using DataLensPatterns.Core.Services;

namespace Lab2_AbstractFactoryPrototype.Factories;

public sealed class CsvToolkitFactory : IAnalysisToolkitFactory
{
    public IDataSetLoader CreateLoader() => new CsvLoader();
    public IAnalysisExporter CreateExporter() => new CsvAnalysisExporter();

    private sealed class CsvLoader : IDataSetLoader
    {
        private readonly CsvDatasetReader _reader = new();
        public TabularDataset Load(string path) => _reader.Load(path);
    }

    private sealed class CsvAnalysisExporter : IAnalysisExporter
    {
        public string Export(AnalysisReport report)
        {
            var lines = new List<string> { "column,kind,values,missing,min,max,average,median" };
            foreach (var c in report.Columns)
                lines.Add(string.Join(",", new[] { Escape(c.Name), c.Kind.ToString(),
                    c.ValueCount.ToString(CultureInfo.InvariantCulture), c.MissingCount.ToString(CultureInfo.InvariantCulture),
                    F(c.Minimum), F(c.Maximum), F(c.Average), F(c.Median) }));
            return string.Join(Environment.NewLine, lines);
        }
        private static string F(double? value) => value?.ToString("0.####", CultureInfo.InvariantCulture) ?? "";
        private static string Escape(string value) => value.Contains(',') || value.Contains('"') || value.Contains('\n') || value.Contains('\r')
            ? "\"" + value.Replace("\"", "\"\"", StringComparison.Ordinal) + "\""
            : value;
    }
}
