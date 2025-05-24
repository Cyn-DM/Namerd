namespace Namerd.Domain.Results;

public class UserAlreadyNominatedResult : OperationResult
{
    public bool? IsUserAlreadyNominated { get; }
    private UserAlreadyNominatedResult(bool isSuccessful, bool? isUserAlreadyNominated, string? errorMessage = null) : base(isSuccessful, errorMessage)
    {
        IsUserAlreadyNominated = isUserAlreadyNominated;
    }
    
    public static UserAlreadyNominatedResult Nominated() => new (true, true);
    public static UserAlreadyNominatedResult NotNominated() => new (true, false);
    public new static UserAlreadyNominatedResult Failure(string errorMessage) => new (false, null, errorMessage);
}