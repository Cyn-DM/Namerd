using Namerd.Persistence.Repository;
using Namerd.Services.MessageCreators;
using NetCord;
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

    public async Task CallSettingsMenu(ApplicationCommandContext context)
    {
        var user = context.User;
        Permissions userPermissions = new Permissions();
        if (user is GuildInteractionUser guildUser)
        {
            userPermissions = guildUser.Permissions;
        }

        if ((userPermissions & Permissions.Administrator) != 0)
        {
            await SettingMessageCreator.CreateSettingMenuMessage(context);
        }
        else
        {
            await SettingMessageCreator.CreateSettingPermissionExceptionMessage(context);
        }
    }

    public async Task CallNominationChannelMenu(StringMenuInteractionContext context)
    {
        var user = context.User;
        Permissions userPermissions = new Permissions();
        if (user is GuildInteractionUser guildUser)
        {
            userPermissions = guildUser.Permissions;
        }

        if ((userPermissions & Permissions.Administrator) != 0)
        {
            await SettingMessageCreator.CreateChannelSettingSelectMessage(context);
        }
        else
        {
            await SettingMessageCreator.CreateSettingPermissionExceptionMessage(context);
        }
    }
    
    public async Task SetNominationChannel(IInteractionContext context, Channel channel)
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
            await SettingMessageCreator.CreateSettingSucceeded(context, "Successfully set nomination channel.");
        }
        else
        {
            await SettingMessageCreator.CreateSettingPermissionExceptionMessage(context);
        }
    }
}