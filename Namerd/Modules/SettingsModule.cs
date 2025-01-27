using Namerd.Services;
using Namerd.Services.MessageCreators;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Namerd.Modules;

public class SettingsModule : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("setofthemonth", "Sets the channel for 'Of The Month' voting. ")]
    public async Task SetOfTheMonthChannel()
    {
        try
        {
            
        }
        catch (RestException ex)
        {
            if (ex.ReasonPhrase != null)
            {
                await GeneralMessageCreator.CreateDiscordExceptionMessage(Context, ex.ReasonPhrase);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            
            var callback = InteractionCallback.Message(
                new InteractionMessageProperties
                {
                    Content = "Got your setting request, but something went wrong.",
                    Flags = MessageFlags.Ephemeral
                }
            );

            await RespondAsync(callback);
        }
    } 
    
}