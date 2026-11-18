using DataLensPatterns.Core.Models;
using DataLensPatterns.Core.Services;

namespace Lab5_FacadeFlyweight.Facade;

public sealed record FacadeAnalysisResult(TabularDataset Dataset, AnalysisReport Report, string CsvExport);

/// <summary>Фасад предоставляет клиенту одну операцию вместо набора подсистемных вызовов.</summary>
public sealed class DatasetAnalysisFacade
{
    private readonly CsvDatasetReader _reader = new();
    private readonly DatasetValidator _validator = new();
    private readonly DatasetAnalyzer _analyzer = new();
    private readonly CsvDatasetExporter _exporter = new();

    public FacadeAnalysisResult AnalyzeCsv(string path, NumericFilter? filter = null)
    {
        var dataset = _reader.Load(path);
        _validator.Validate(dataset);
        if (filter is not null) dataset = _analyzer.FilterNumeric(dataset, filter);
        var report = _analyzer.Analyze(dataset);
        var csv = _exporter.Export(dataset);
        return new FacadeAnalysisResult(dataset, report, csv);
    }
}
