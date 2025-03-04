using Namerd.Persistence.Repository;
using Namerd.Services.MessageCreators;
using NetCord;
using NetCord.Services.ApplicationCommands;

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
    
    public async Task SetNominationChannel(ApplicationCommandContext context)
    {
        var user = context.User;
        Permissions userPermissions = new Permissions();
        if (user is GuildInteractionUser guildUser)
        {
            userPermissions = guildUser.Permissions;
        }

        if ((userPermissions & Permissions.Administrator) != 0)
        {
            await _botRepository.SetNominationChannel(context.Guild.Id, context.Channel.Id);
            await SettingMessageCreator.CreateSettingSucceeded(context, "Successfully set nomination channel.");
        }
        else
        {
            await SettingMessageCreator.CreateSettingPermissionExceptionMessage(context);
        }
    }
}