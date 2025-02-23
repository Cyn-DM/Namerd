using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Namerd.Services.MessageCreators;

public static class SettingMessageCreator
{
    public static async Task CreateSettingPermissionExceptionMessage(ApplicationCommandContext context)
    {
        var embed = new EmbedProperties()
            .WithTitle($"Sorry, could not process your request. You do not have the necessary permissions.")
            .WithColor(new (0xE8004F));
        
        var messageProperties = new MessageProperties
        {
            Embeds = [embed]
        };

        await context.Channel.SendMessageAsync(messageProperties);
    }
    
    public static async Task CreateSettingSucceeded(ApplicationCommandContext context, string message)
    {
        var embed = new EmbedProperties()
            .WithTitle($"Successfully changed the setting!")
            .WithDescription(message)
            .WithColor(new (0xE8004F));
        
        var messageProperties = new MessageProperties
        {
            Embeds = [embed]
        };

        await context.Channel.SendMessageAsync(messageProperties);
    }
}