using NetCord;
using NetCord.Gateway;
using NetCord.Services.ApplicationCommands;
using IApplicationCommandContext = Namerd.Application.Interfaces.IApplicationCommandContext;

namespace Namerd.Application.Wrappers;

public class ProductionApplicationCommandContext(ApplicationCommandContext context) : IApplicationCommandContext
{
    public Guild? Guild { get; } = context.Guild;
    public TextChannel Channel { get; } = context.Channel;
    public User User { get; } = context.User;
    public GatewayClient Client { get; } = context.Client;
    public Interaction Interaction { get; } = context.Interaction;
}