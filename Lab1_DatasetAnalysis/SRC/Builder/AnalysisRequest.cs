using DataLensPatterns.Core.Models;

namespace Lab1_BuilderSingleton.Builder;

public sealed class AnalysisRequest
{
    public string FilePath { get; }
    public bool IncludeStatistics { get; }
    public NumericFilter? Filter { get; }

    public AnalysisRequest(string filePath, bool includeStatistics, NumericFilter? filter)
    {
        FilePath = filePath;
        IncludeStatistics = includeStatistics;
        Filter = filter;
    }
}
