using Lab4_CompositeDecorator.Composite;
using Lab4_CompositeDecorator.Decorators;
using DataLensPatterns.Core.Services;
using DataLensPatterns.Core.Utilities;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("ЛАБОРАТОРНАЯ №4. КОМПОНОВЩИК И ДЕКОРАТОР — ВАРИАНТ 13\n");

var all = new CsvDatasetReader().Load(SampleDataLocator.Find());
var firstPart = all.TakeRows(0, 5, "sales_part_1");
var secondPart = all.TakeRows(5, all.Rows.Count - 5, "sales_part_2");
var root = new DatasetGroup("Общий набор продаж");
root.Add(new DatasetLeaf("Первая часть", firstPart));
var nested = new DatasetGroup("Вторая группа данных");
nested.Add(new DatasetLeaf("Вторая часть", secondPart));
root.Add(nested);
var combined = root.GetDataset();
var report = new DatasetAnalyzer().Analyze(combined);

Console.WriteLine("[Composite]");
Console.WriteLine($"Дерево компонентов: {root.Name} → {root.Children.Count} дочерних узла; один из узлов является вложенной группой.");
Console.WriteLine($"Агрегированный набор: {combined.Rows.Count} строк, {combined.Columns.Count} столбцов.\n");

Console.WriteLine("[Decorator]");
IAnalysisReportFormatter formatter = new BasicReportFormatter();
formatter = new MissingValuesDecorator(formatter);
formatter = new NumericStatisticsDecorator(formatter);
Console.WriteLine(formatter.Format(report));
