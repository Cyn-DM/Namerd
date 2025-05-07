using Namerd.Application.Interfaces;
using NetCord;
using NetCord.Gateway;
using NetCord.Services.ApplicationCommands;

namespace Namerd.Application.Wrappers;

public class ProductionApplicationCommandContextWrapper(ApplicationCommandContext context) : IApplicationCommandContextWrapper
{
    public Guild? Guild { get; } = context.Guild;
    public TextChannel Channel { get; } = context.Channel;
    public IUserWrapper UserWrapper { get; } = new ProductionUserWrapper(context.User);
    public GatewayClient Client { get; } = context.Client;
    public IInteractionWrapper InteractionWrapper { get; } = new ProductionInteractionWrapper(context.Interaction);
}