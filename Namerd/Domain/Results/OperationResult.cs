namespace Namerd.Domain.Results;

public class OperationResult
{
    public bool IsSuccessful { get; }
    public string? ErrorMessage { get; }

    protected OperationResult(bool isSuccessful, string? errorMessage = null)
    {
        IsSuccessful = isSuccessful;
        ErrorMessage = errorMessage;
    }

    public static OperationResult Success() => new(true);

    public static OperationResult Failure(string errorMessage) => new(false, errorMessage);
}