using Namerd.CustomExceptions;
using Namerd.Services;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Namerd.Modules;

public class NicknameModule : ApplicationCommandModule<ApplicationCommandContext>
{

    
    [SlashCommand("namevote", "Starts a nickname vote")]
    public async Task<string> StartVote(GuildUser user, string nickname)
    {

        try
        {
            var context = Context;
            await NicknameService.VoteForNickName(context, user, nickname);

            return "Success";
        }
        catch (UserIsOwnerException ex)
        {
            return ex.Message;
        }
        catch (Exception e)
        {
            return "Failed";
        }
    }

   

}