using Lab3_AdapterBridge.Adapter;
using Lab3_AdapterBridge.Bridge;
using DataLensPatterns.Core.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("ЛАБОРАТОРНАЯ №3. АДАПТЕР И МОСТ — ВАРИАНТ 13\n");

IDatasetSource source = new LegacyDatasetAdapter(new LegacySalesTableProvider());
var dataset = source.Load();
var report = new DatasetAnalyzer().Analyze(dataset);
Console.WriteLine("[Adapter] Старый источник string[][] подключён к новой модели TabularDataset.");
Console.WriteLine($"Преобразовано строк: {dataset.Rows.Count}; столбцов: {dataset.Columns.Count}.\n");

Console.WriteLine("[Bridge] Одна абстракция представления, два рендерера:");
var compactText = new SummaryReportView(new PlainTextReportRenderer());
var detailedMarkdown = new DetailedReportView(new MarkdownReportRenderer());
Console.WriteLine("--- Краткий текстовый отчёт ---");
Console.WriteLine(compactText.Show(report));
Console.WriteLine("--- Подробный Markdown-отчёт ---");
Console.WriteLine(detailedMarkdown.Show(report));
