using System.Globalization;
using Lab5_FacadeFlyweight.Facade;
using Lab5_FacadeFlyweight.Flyweight;
using DataLensPatterns.Core.Models;
using DataLensPatterns.Core.Utilities;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("ЛАБОРАТОРНАЯ №5. ФАСАД И ПРИСПОСОБЛЕНЕЦ — ВАРИАНТ 13");
Console.WriteLine("Интерактивный анализ табличного набора данных\n");

var samplePath = SampleDataLocator.Find();
Console.WriteLine($"Демонстрационный файл: {samplePath}");
Console.Write("Числовой столбец для фильтра [revenue]: ");
var columnName = Console.ReadLine();
if (string.IsNullOrWhiteSpace(columnName)) columnName = "revenue";
var minimum = ReadNumber("Минимальное значение [20000]: ", 20000);
var maximum = ReadNumber("Максимальное значение [90000]: ", 90000);
if (minimum > maximum)
{
    Console.WriteLine("Минимум больше максимума. Используется диапазон по умолчанию 20000–90000.");
    minimum = 20000;
    maximum = 90000;
}

var facade = new DatasetAnalysisFacade();
var result = facade.AnalyzeCsv(samplePath, new NumericFilter(columnName, minimum, maximum));
Console.WriteLine("\n[Facade] Один вызов выполнил загрузку, проверку, фильтрацию, анализ и подготовку CSV.");
Console.WriteLine($"Итог: {result.Report.RowCount} строк; заполненность={result.Report.CompletenessPercent:F2}%.");
Console.WriteLine("\n[Flyweight] Повторно используемые стили столбцов:");
var styles = new ColumnStyleFactory();
new ColumnStyleRenderer(styles).Render(result.Report);
var firstNumeric = styles.GetStyle(ColumnDataKind.Numeric);
var secondNumeric = styles.GetStyle(ColumnDataKind.Numeric);
Console.WriteLine($"Один объект стиля для одинакового типа: {ReferenceEquals(firstNumeric, secondNumeric)}");
Console.WriteLine($"Создано общих стилей: {styles.CreatedCount}; столбцов в отчёте: {result.Report.ColumnCount}.");
Console.WriteLine("\nЭкспорт CSV подготовлен; первые строки:");
Console.WriteLine(string.Join(Environment.NewLine, result.CsvExport.Split(Environment.NewLine).Take(4)));

static double ReadNumber(string prompt, double fallback)
{
    Console.Write(prompt);
    var value = Console.ReadLine();
    return double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed) && double.IsFinite(parsed)
        ? parsed : fallback;
}
