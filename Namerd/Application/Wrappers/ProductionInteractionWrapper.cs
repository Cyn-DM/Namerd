using Namerd.Application.Interfaces;
using NetCord;
using NetCord.Gateway;

namespace Namerd.Application.Wrappers;

public class ProductionInteractionWrapper : IInteractionWrapper
{
    private readonly Interaction _interaction;

    public ProductionInteractionWrapper(Interaction interaction)
    {
        _interaction = interaction;
    }

    public IUserWrapper UserWrapper => new ProductionUserWrapper(_interaction.User);
    public Guild? Guild => _interaction.Guild;
    public TextChannel Channel => _interaction.Channel;
}