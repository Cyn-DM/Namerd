namespace Namerd.CustomExceptions;

public class UserIsOwnerException : Exception
{
    public override string Message => "You can't change the server owners properties.";
}