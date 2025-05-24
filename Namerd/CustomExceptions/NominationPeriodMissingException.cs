using NetCord.Gateway;

namespace Namerd.CustomExceptions;

public class NominationPeriodMissingException : Exception
{
    public override string Message => "There is no nomination period running.";
}