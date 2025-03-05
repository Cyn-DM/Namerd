using Namerd.Services;
using NetCord;
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
    public async Task ChooseSetting()
    {
        var option = Context.SelectedValues.FirstOrDefault().ToString();
        switch (option)
        {
            default: return;
            case "monthlyNominationChannel":
                await _settingsService.CallNominationChannelMenu(Context);
                break;
        }
    }
}