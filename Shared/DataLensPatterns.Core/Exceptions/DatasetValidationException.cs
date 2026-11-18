namespace DataLensPatterns.Core.Exceptions;

public sealed class DatasetValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }

    public DatasetValidationException(IEnumerable<string> errors)
        : base(string.Join("; ", errors))
    {
        Errors = errors.ToArray();
    }
}
