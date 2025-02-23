using NetCord;
using NetCord.Services.ApplicationCommands;

namespace Namerd.Services;

public class SettingsService
{
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
            // Save settings here
        }
        
        var channel = context.Channel;
    }
}