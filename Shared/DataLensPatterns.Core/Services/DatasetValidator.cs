using DataLensPatterns.Core.Exceptions;
using DataLensPatterns.Core.Models;

namespace DataLensPatterns.Core.Services;

public sealed class DatasetValidator
{
    public void Validate(TabularDataset dataset, int maxRows = 20000, int maxColumns = 100)
    {
        ArgumentNullException.ThrowIfNull(dataset);
        var errors = new List<string>();
        if (dataset.Rows.Count > maxRows) errors.Add($"Количество строк превышает лимит {maxRows}.");
        if (dataset.Columns.Count > maxColumns) errors.Add($"Количество столбцов превышает лимит {maxColumns}.");
        if (dataset.Columns.Count == 0) errors.Add("Набор не содержит столбцов.");
        if (errors.Count > 0) throw new DatasetValidationException(errors);
    }
}
