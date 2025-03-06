using Namerd.Persistence.Repository;
using Namerd.Services.MessageCreators;
using NetCord;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.ComponentInteractions;

namespace Namerd.Services;

public class SettingsService
{
    private readonly BotRepository _botRepository;

    public SettingsService(BotRepository botRepository)
    {
        _botRepository = botRepository;

    }

    public static InteractionMessageProperties CallSettingsMenu(ApplicationCommandContext context)
    {
        var user = context.User;
        Permissions userPermissions = new Permissions();
        if (user is GuildInteractionUser guildUser)
        {
            userPermissions = guildUser.Permissions;
        }

        if ((userPermissions & Permissions.Administrator) != 0)
        {
            return SettingMessageCreator.CreateSettingMenuMessage(context);
        }
        else
        {
            return SettingMessageCreator.CreateSettingPermissionExceptionMessage();
        }
    }

    public static InteractionMessageProperties CallNominationChannelMenu(StringMenuInteractionContext context)
    {
        var user = context.User;
        Permissions userPermissions = new Permissions();
        if (user is GuildInteractionUser guildUser)
        {
            userPermissions = guildUser.Permissions;
        }

        if ((userPermissions & Permissions.Administrator) != 0)
        {
            return SettingMessageCreator.CreateNominationChannelSelectMessage(context);
        }
        else
        {
            return SettingMessageCreator.CreateSettingPermissionExceptionMessage();
        }
    }
    
    public async Task<InteractionMessageProperties> SetNominationChannel(IInteractionContext context, Channel channel)
    {
        var user = context.Interaction.User;
        Permissions userPermissions = new Permissions();
        if (user is GuildInteractionUser guildUser)
        {
            userPermissions = guildUser.Permissions;
        }

        if ((userPermissions & Permissions.Administrator) != 0)
        {
            await _botRepository.SetNominationChannel(context.Interaction.Guild.Id, channel.Id);
            return SettingMessageCreator.CreateSettingSucceeded("Successfully set nomination channel.");
        }
        else
        {
            return SettingMessageCreator.CreateSettingPermissionExceptionMessage();
        }
    }
}