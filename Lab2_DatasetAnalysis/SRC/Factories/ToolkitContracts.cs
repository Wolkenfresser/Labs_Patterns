using DataLensPatterns.Core.Models;

namespace Lab2_AbstractFactoryPrototype.Factories;

public interface IDataSetLoader { TabularDataset Load(string path); }
public interface IAnalysisExporter { string Export(AnalysisReport report); }
public interface IAnalysisToolkitFactory
{
    IDataSetLoader CreateLoader();
    IAnalysisExporter CreateExporter();
}
