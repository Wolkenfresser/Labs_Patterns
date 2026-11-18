using Lab2_AbstractFactoryPrototype.Factories;
using Lab2_AbstractFactoryPrototype.Prototype;
using DataLensPatterns.Core.Services;
using DataLensPatterns.Core.Utilities;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("ЛАБОРАТОРНАЯ №2. АБСТРАКТНАЯ ФАБРИКА И ПРОТОТИП — ВАРИАНТ 13\n");

var analyzer = new DatasetAnalyzer();
IAnalysisToolkitFactory csvFactory = new CsvToolkitFactory();
var csvData = csvFactory.CreateLoader().Load(SampleDataLocator.Find("sample_sales.csv"));
var csvReport = analyzer.Analyze(csvData);
Console.WriteLine("[Abstract Factory: CSV]");
Console.WriteLine($"Фабрика создала совместимые продукты: {csvData.Rows.Count} строк, экспортёр={csvFactory.CreateExporter().GetType().Name}");
Console.WriteLine(csvFactory.CreateExporter().Export(csvReport).Split(Environment.NewLine)[0]);

IAnalysisToolkitFactory jsonFactory = new JsonToolkitFactory();
var jsonData = jsonFactory.CreateLoader().Load(SampleDataLocator.Find("sample_sales.json"));
var jsonReport = analyzer.Analyze(jsonData);
Console.WriteLine("\n[Abstract Factory: JSON]");
Console.WriteLine($"Загружено: {jsonReport.RowCount} строк; заполненность={jsonReport.CompletenessPercent:F2}%");
var jsonExport = jsonFactory.CreateExporter().Export(jsonReport);
Console.WriteLine(jsonExport.Substring(0, Math.Min(160, jsonExport.Length)));

Console.WriteLine("\n[Prototype]");
var catalog = new AnalysisProfileRegistry();
catalog.Register("revenue", new AnalysisProfile("Продажи", new[] { "region", "product", "revenue" }, new DataLensPatterns.Core.Models.NumericFilter("revenue", 20000, 90000)));
var profileA = catalog.Create("revenue");
var profileB = profileA.Clone();
profileB.SelectedColumns.Add("discount");
Console.WriteLine($"Исходный профиль: {string.Join(", ", profileA.SelectedColumns)}");
Console.WriteLine($"Копия профиля: {string.Join(", ", profileB.SelectedColumns)}");
Console.WriteLine($"Списки независимы: {!ReferenceEquals(profileA.SelectedColumns, profileB.SelectedColumns)}");
