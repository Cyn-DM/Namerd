using Namerd.Services;
using Namerd.Services.MessageCreators;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace Namerd.Modules;

public class SettingsChannelSelectComponentModule : ComponentInteractionModule<ChannelMenuInteractionContext>
{
    private readonly SettingsService _settingsService;

    public SettingsChannelSelectComponentModule(SettingsService settingsService)
    {
        _settingsService = settingsService;
    }
    
    [ComponentInteraction("nominationChannelSelect")]
    public async Task SetNominationChannel()
    {
        try
        {
           await _settingsService.SetNominationChannel(Context, Context.SelectedChannels.FirstOrDefault());
        }
        catch (RestException ex)
        {
            if (ex.ReasonPhrase != null)
            {
                await GeneralMessageCreator.CreateDiscordExceptionMessage(Context, ex.ReasonPhrase);
            }
            else
            {
                await GeneralMessageCreator.CreateDiscordExceptionMessage(Context, "Unknown Error");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            
            var callback = InteractionCallback.Message(
                new InteractionMessageProperties
                {
                    Content = "Got your request, but something went wrong.",
                    Flags = MessageFlags.Ephemeral
                }
            );

            await RespondAsync(callback);
        }
    }
}