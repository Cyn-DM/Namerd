using NetCord;
using NetCord.Gateway;

namespace Namerd.Application.Interfaces;

public interface IInteractionWrapper
{
    IUserWrapper UserWrapper { get; }
    Guild? Guild { get; }
    TextChannel Channel { get; }
}