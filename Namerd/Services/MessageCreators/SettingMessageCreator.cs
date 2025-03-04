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
    
    public static async Task CreateSettingMenuMessage(ApplicationCommandContext context)
    {
        var embed = new EmbedProperties()
            .WithTitle($"Choose the setting you want to change.")
            .WithColor(new (0xE8004F));

        var settingOptions = new[]
        {
            new StringMenuSelectOptionProperties("Monthly Nomination Channel", "monthlyNominationChannel"),
        };

        var components = new[]
        {
            new StringMenuProperties("settingsMenu", settingOptions),
        };
        
        var messageProperties = new MessageProperties
        {
            Embeds = [embed],
            Components = components,
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