using Namerd.Application.Interfaces;
using NetCord;

namespace Namerd.Application.Wrappers;

public class ProductionUserWrapper : IUserWrapper
{
    private readonly User _user;

    public ProductionUserWrapper(User user)
    {
        _user = user;
    }

    public string? Username => _user.Username;
    public ulong Id => _user.Id;
}