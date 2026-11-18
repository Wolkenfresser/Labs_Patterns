using DataLensPatterns.Core.Models;

namespace Lab1_BuilderSingleton.Builder;

/// <summary>Пошаговое создание сложного запроса анализа.</summary>
public sealed class AnalysisRequestBuilder
{
    private string? _filePath;
    private bool _includeStatistics = true;
    private NumericFilter? _filter;

    public AnalysisRequestBuilder Reset()
    {
        _filePath = null;
        _includeStatistics = true;
        _filter = null;
        return this;
    }

    public AnalysisRequestBuilder FromFile(string filePath) { _filePath = filePath; return this; }
    public AnalysisRequestBuilder IncludeStatistics(bool value = true) { _includeStatistics = value; return this; }
    public AnalysisRequestBuilder WithNumericFilter(string column, double min, double max)
    {
        _filter = new NumericFilter(column, min, max);
        return this;
    }
    public AnalysisRequest Build()
    {
        if (string.IsNullOrWhiteSpace(_filePath))
            throw new InvalidOperationException("Перед построением запроса необходимо указать файл.");
        if (_filter is not null && _filter.Minimum > _filter.Maximum)
            throw new InvalidOperationException("Минимальная граница фильтра не может превышать максимальную.");
        return new AnalysisRequest(_filePath, _includeStatistics, _filter);
    }
}
