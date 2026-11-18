namespace DataLensPatterns.Core.Utilities;

public static class SampleDataLocator
{
    public static string Find(string fileName = "sample_sales.csv")
    {
        var current = new DirectoryInfo(Environment.CurrentDirectory);
        for (var depth = 0; current is not null && depth < 8; depth++, current = current.Parent)
        {
            var candidate = Path.Combine(current.FullName, "datasets", fileName);
            if (File.Exists(candidate)) return candidate;
        }
        var baseDirectory = new DirectoryInfo(AppContext.BaseDirectory);
        for (var depth = 0; baseDirectory is not null && depth < 10; depth++, baseDirectory = baseDirectory.Parent)
        {
            var candidate = Path.Combine(baseDirectory.FullName, "datasets", fileName);
            if (File.Exists(candidate)) return candidate;
        }
        throw new FileNotFoundException($"Не найден демонстрационный файл datasets/{fileName}. Запускайте приложение из корня решения.");
    }
}
