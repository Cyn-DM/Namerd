namespace Namerd.Application.Interfaces;

public interface IUserWrapper
{
    string? Username { get; }
    ulong Id { get; }
}