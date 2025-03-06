using Namerd.CustomExceptions;
using Namerd.Services;
using Namerd.Services.MessageCreators;
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
                NicknameService.VoteForNickName(Context, user, nickname, timeInMinutes)
                );
            
            await RespondAsync(callback);

            var message = await GetResponseAsync();
            
            await message.AddReactionAsync("👍");
            await message.AddReactionAsync("👎");

            await NicknameService.MentionUserAsync(Context, user);
            
            await NicknameService.ProcessVoting(message.Id, timeInMinutes, Context, user, nickname);
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