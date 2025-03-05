using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;

namespace Namerd.Services.MessageCreators
{
    public static class GeneralMessageCreator
    {
        public static async Task CreateDiscordExceptionMessage(IInteractionContext context, string discordException)
        {
            var embed = new EmbedProperties()
                .WithTitle($"Sorry, could not process your request. Discord exception:")
                .WithDescription(discordException)
                .WithColor(new (0xE8004F));
        
            var messageProperties = new MessageProperties
            {
                Embeds = [embed]
            };

            await context.Interaction.Channel.SendMessageAsync(messageProperties);
        }
    }
}