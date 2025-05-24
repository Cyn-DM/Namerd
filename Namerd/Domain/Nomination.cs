namespace Namerd.Domain;

public class Nomination
{
    public ulong UserId { get;}
    public string NominationReason { get; }
    
    public Guid NominationPeriodId { get; set; }
    public NominationPeriod NominationPeriod { get; set; }
    

    public Nomination(string nominationReason, ulong userId)
    {
        this.NominationReason = nominationReason;
        this.UserId = userId;
    }
    
    public NominationValidationResult IsValidNomination()
    {
        if (string.IsNullOrEmpty(NominationReason))
        {
            return new NominationValidationResult(false, NominationValidationResultType.NoReason);
        }

        if (NominationReason.Length > 280)
        {
            return new NominationValidationResult(false, NominationValidationResultType.TooLongReason);
        }
        
        return new NominationValidationResult(true, NominationValidationResultType.Valid);
    }
}

public class NominationValidationResult
{
    bool IsValid { get; }
    NominationValidationResultType ResultType { get; }
    
    public NominationValidationResult(bool isValid, NominationValidationResultType resultType)
    {
        IsValid = isValid;
        ResultType = resultType;
    }
}

public enum NominationValidationResultType
{
    Valid,
    AlreadyNominated,
    NoReason,
    TooLongReason,
}