namespace Lab3_AdapterBridge.Adapter;

/// <summary>Старый источник отдаёт двумерный массив строк без модели набора данных.</summary>
public interface ILegacyTableProvider { string[][] GetRows(); }
public interface IDatasetSource { DataLensPatterns.Core.Models.TabularDataset Load(); }

public sealed class LegacySalesTableProvider : ILegacyTableProvider
{
    public string[][] GetRows() => new[]
    {
        new[] { "id", "region", "product", "units", "revenue", "discount" },
        new[] { "1001", "North", "Keyboard", "3", "85000", "0.05" },
        new[] { "1002", "South", "Mouse", "5", "24000", "0.10" },
        new[] { "1003", "West", "Monitor", "2", "56000", "" },
        new[] { "1004", "East", "Laptop", "1", "90000", "0.00" },
        new[] { "1005", "North", "Mouse", "4", "26000", "0.15" }
    };
}
