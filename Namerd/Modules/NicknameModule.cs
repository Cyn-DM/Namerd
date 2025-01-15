using Namerd.CustomExceptions;
using Namerd.Services;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Namerd.Modules;

public class NicknameModule : ApplicationCommandModule<ApplicationCommandContext>
{
    
    [SlashCommand("nicknamevote", "Starts a nickname vote")]
    public async Task StartVote(GuildUser user, string nickname, int timeInMinutes)
    {
        try
        {
            var callback = InteractionCallback.Message(
                new InteractionMessageProperties
                {
                    Content = "Got your vote request!",
                    Flags = MessageFlags.Ephemeral
                }
            );

            await RespondAsync(callback);
            
            var context = Context;

            if (await NicknameService.ValidateNickname(context, nickname) && await NicknameService.CheckTime(context, timeInMinutes))
            {
                await NicknameService.VoteForNickName(context, user, nickname, timeInMinutes);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            
            var callback = InteractionCallback.Message(
                new InteractionMessageProperties
                {
                    Content = "Got your vote request, but something went wrong.",
                    Flags = MessageFlags.Ephemeral
                }
            );

            await RespondAsync(callback);
        }
    }
    
}