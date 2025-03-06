using NetCord;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;

namespace Namerd.Services.MessageCreators
{
    public static class GeneralMessageCreator
    {
        public static InteractionMessageProperties CreateDiscordExceptionMessage(IInteractionContext context, string discordException)
        {
            var embed = new EmbedProperties()
                .WithTitle($"Sorry, could not process your request. Discord exception:")
                .WithDescription(discordException)
                .WithColor(new (0xE8004F));
        
            var messageProperties = new InteractionMessageProperties()
            {
                Embeds = [embed],
                Flags = MessageFlags.Ephemeral
            };

            return messageProperties;
        }

        public static InteractionMessageProperties CreateUnknownErrorMessage()
        {
            var messageProperties = new InteractionMessageProperties
            {
                Content = "Got your request, but something went wrong.",
                Flags = MessageFlags.Ephemeral
            };
            return messageProperties;
        }
    }
}