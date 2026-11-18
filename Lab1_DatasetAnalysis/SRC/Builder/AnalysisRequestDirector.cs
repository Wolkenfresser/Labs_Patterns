namespace Lab1_BuilderSingleton.Builder;

/// <summary>Готовые конфигурации строителя для типовых сценариев.</summary>
public sealed class AnalysisRequestDirector(AnalysisRequestBuilder builder)
{
    public AnalysisRequest CreateFullAnalysis(string path) => builder.Reset().FromFile(path)
        .IncludeStatistics(true).Build();

    public AnalysisRequest CreateRevenuePreview(string path) => builder.Reset().FromFile(path)
        .IncludeStatistics(true).WithNumericFilter("revenue", 20000, 90000).Build();
}
