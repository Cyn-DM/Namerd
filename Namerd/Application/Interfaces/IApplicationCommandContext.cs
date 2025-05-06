using NetCord;
using NetCord.Gateway;
using NetCord.Services;

namespace Namerd.Application.Interfaces;

public interface IApplicationCommandContext : IInteractionContext
{
    public Guild? Guild { get; }
    public TextChannel Channel { get; }
    public User User { get; }
}