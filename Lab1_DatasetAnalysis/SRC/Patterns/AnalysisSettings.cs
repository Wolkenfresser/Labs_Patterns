namespace Lab1_BuilderSingleton.Patterns;

/// <summary>Потокобезопасный ленивый Singleton с общими ограничениями анализа.</summary>
public sealed class AnalysisSettings
{
    private static readonly Lazy<AnalysisSettings> Holder = new(() => new AnalysisSettings());
    private AnalysisSettings() { }
    public static AnalysisSettings Instance => Holder.Value;
    public int MaxRows { get; } = 20000;
    public int MaxColumns { get; } = 100;
    public string DefaultEncoding { get; } = "UTF-8";
}
