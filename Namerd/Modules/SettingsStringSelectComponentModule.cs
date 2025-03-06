using Namerd.Services;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace Namerd.Modules;

public class SettingsStringSelectComponentModule : ComponentInteractionModule<StringMenuInteractionContext>
{
    private readonly SettingsService _settingsService;

    public SettingsStringSelectComponentModule(SettingsService settingsService)
    {
        _settingsService = settingsService;
    }
    
    [ComponentInteraction("settingsMenu")]
    public InteractionMessageProperties ChooseSetting()
    {
        var option = Context.SelectedValues.FirstOrDefault().ToString();
        
        switch (option)
        {
            
            case "monthlyNominationChannel":
                return SettingsService.CallNominationChannelMenu(Context);
            default:
                return new InteractionMessageProperties()
                {
                    Content = "Please select a valid option",
                };
        }
    }
}