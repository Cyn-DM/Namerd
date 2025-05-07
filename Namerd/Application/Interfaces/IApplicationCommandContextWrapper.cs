using NetCord;
using NetCord.Gateway;

namespace Namerd.Application.Interfaces;

public interface IApplicationCommandContextWrapper : IInteractionContextWrapper
{
    public Guild? Guild { get; }
    public TextChannel Channel { get; }
    public IUserWrapper UserWrapper { get; }
}