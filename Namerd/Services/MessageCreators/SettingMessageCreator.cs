using NetCord;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.ComponentInteractions;

namespace Namerd.Services.MessageCreators;

public static class SettingMessageCreator
{
    public static InteractionMessageProperties CreateSettingPermissionExceptionMessage()
    {
        var embed = new EmbedProperties()
            .WithTitle($"Sorry, could not process your request. You do not have the necessary permissions.")
            .WithColor(new (0xE8004F));
        
        var messageProperties = new InteractionMessageProperties
        {
            Embeds = [embed],
            Flags = MessageFlags.Ephemeral
        };

        return messageProperties;
    }
    
    public static InteractionMessageProperties CreateSettingMenuMessage(IInteractionContext context)
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
        
        var messageProperties = new InteractionMessageProperties
        {
            Embeds = [embed],
            Components = components,
        };

        return messageProperties;
    }
    
    public static InteractionMessageProperties CreateSettingSucceeded(string message)
    {
        var embed = new EmbedProperties()
            .WithTitle($"Successfully changed the setting!")
            .WithDescription(message)
            .WithColor(new (0xE8004F));
        
        var messageProperties = new InteractionMessageProperties
        {
            Embeds = [embed]
        };

        return messageProperties;
    }

    public static InteractionMessageProperties CreateNominationChannelSelectMessage(IInteractionContext context)
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

        var messageProperties = new InteractionMessageProperties()
        {
            Embeds = [embed],
            Components = components,
        };
        
        return messageProperties;
    }
}