using NetCord;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.ComponentInteractions;

namespace Namerd.Services.MessageCreators;

public static class SettingMessageCreator
{
    public static async Task CreateSettingPermissionExceptionMessage(IInteractionContext context)
    {
        var embed = new EmbedProperties()
            .WithTitle($"Sorry, could not process your request. You do not have the necessary permissions.")
            .WithColor(new (0xE8004F));
        
        var messageProperties = new MessageProperties
        {
            Embeds = [embed]
        };

        await context.Interaction.Channel.SendMessageAsync(messageProperties);
    }
    
    public static async Task CreateSettingMenuMessage(IInteractionContext context)
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

        await context.Interaction.Channel.SendMessageAsync(messageProperties);
    }
    
    public static async Task CreateSettingSucceeded(IInteractionContext context, string message)
    {
        var embed = new EmbedProperties()
            .WithTitle($"Successfully changed the setting!")
            .WithDescription(message)
            .WithColor(new (0xE8004F));
        
        var messageProperties = new MessageProperties
        {
            Embeds = [embed]
        };

        await context.Interaction.Channel.SendMessageAsync(messageProperties);
    }

    public static async Task CreateChannelSettingSelectMessage(IInteractionContext context)
    {
        var embed = new EmbedProperties()
            .WithTitle($"Please choose the channel you want to set the nomination channel to.")
            .WithColor(new (0xE8004F));
        
        var channelComponent = new ChannelMenuProperties("nominationChannelSelect")
            .AddChannelTypes(ChannelType.TextGuildChannel)
            .WithPlaceholder("Select a channel.");
        
        var components = new[]
        {
            channelComponent
        };

        var messageProperties = new MessageProperties()
        {
            Embeds = [embed],
            Components = components,
        };
        
        await context.Interaction.Channel.SendMessageAsync(messageProperties);
    }
}