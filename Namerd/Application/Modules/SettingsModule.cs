using Namerd.Application.Services;
using Namerd.Application.Services.MessageCreators;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Namerd.Application.Modules;

public class SettingsModule : ApplicationCommandModule<ApplicationCommandContext>
{

    [SlashCommand("settings", "Open the settings menu.")]
    public async Task CallSettingsMenu()
    {
        try
        {
           
            var callback = InteractionCallback.Message(
                SettingsService.CallSettingsMenu(Context)
                );

            await RespondAsync(callback);
        }
        catch (RestException ex)
        {
            if (ex.ReasonPhrase != null)
            {
                var callback = InteractionCallback.Message(
                    GeneralMessageCreator.CreateDiscordExceptionMessage(Context, ex.ReasonPhrase)
                    );

                await RespondAsync(callback);
            }
            else
            {
                var callback = InteractionCallback.Message(
                    GeneralMessageCreator.CreateDiscordExceptionMessage(Context, "Unknown Error")
                    );

                await RespondAsync(callback);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            
            var callback = InteractionCallback.Message(
                GeneralMessageCreator.CreateUnknownErrorMessage()
            );

            await RespondAsync(callback);
        }
    }

    

}