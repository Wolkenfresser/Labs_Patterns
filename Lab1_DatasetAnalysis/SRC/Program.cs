using Lab1_BuilderSingleton.Builder;
using Lab1_BuilderSingleton.Patterns;
using DataLensPatterns.Core.Services;
using DataLensPatterns.Core.Utilities;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("ЛАБОРАТОРНАЯ №1. ПОРОЖДАЮЩИЕ ПАТТЕРНЫ: ОДИНОЧКА И СТРОИТЕЛЬ");
Console.WriteLine("ВАРИАНТ 13 — ИНТЕРАКТИВНЫЙ АНАЛИЗ НАБОРА ДАННЫХ\n");

var settings1 = AnalysisSettings.Instance;
var settings2 = AnalysisSettings.Instance;
Console.WriteLine("[Singleton]");
Console.WriteLine($"Один экземпляр настроек: {ReferenceEquals(settings1, settings2)}");
Console.WriteLine($"Лимиты: строк={settings1.MaxRows}, столбцов={settings1.MaxColumns}, кодировка={settings1.DefaultEncoding}");

var samplePath = SampleDataLocator.Find();
var director = new AnalysisRequestDirector(new AnalysisRequestBuilder());
var request = director.CreateRevenuePreview(samplePath);
var reader = new CsvDatasetReader();
var validator = new DatasetValidator();
var analyzer = new DatasetAnalyzer();
var dataset = reader.Load(request.FilePath);
validator.Validate(dataset, settings1.MaxRows, settings1.MaxColumns);
var result = request.Filter is null ? dataset : analyzer.FilterNumeric(dataset, request.Filter);
var report = analyzer.Analyze(result);

Console.WriteLine("\n[Builder]");
Console.WriteLine($"Файл: {request.FilePath}");
Console.WriteLine($"Фильтр: {request.Filter?.ColumnName} от {request.Filter?.Minimum:N0} до {request.Filter?.Maximum:N0}");
Console.WriteLine($"Результат: {report.RowCount} строк, {report.ColumnCount} столбцов, заполненность {report.CompletenessPercent:F2}%");
foreach (var column in report.Columns.Where(c => c.Kind == DataLensPatterns.Core.Models.ColumnDataKind.Numeric))
    Console.WriteLine($"{column.Name}: min={column.Minimum:N2}; max={column.Maximum:N2}; среднее={column.Average:N2}; медиана={column.Median:N2}");
