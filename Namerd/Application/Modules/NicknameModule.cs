using Namerd.Application.Services;
using Namerd.Application.Services.MessageCreators;
using Namerd.Application.Wrappers;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Namerd.Application.Modules;

public class NicknameModule : ApplicationCommandModule<ApplicationCommandContext>
{
    
    [SlashCommand("nicknamevote", "Starts a nickname vote")]
    public async Task StartVote(GuildUser guildUser, string nickname, int timeInMinutes)
    {
        try
        {
            var context = new ProductionApplicationCommandContextWrapper(Context);
            var user = new ProductionGuildUserWrapper(guildUser);
            var callback = InteractionCallback.Message(
                NicknameService.VoteForNickname(context, user, nickname, timeInMinutes).MessageProperties
                );
            
            await RespondAsync(callback);

            var message = await GetResponseAsync();
            
            await message.AddReactionAsync("👍");
            await message.AddReactionAsync("👎");
            
            await NicknameService.MentionUserAsync(context, user);
            
            await NicknameService.ProcessVoting(message.Id, timeInMinutes, context, user, nickname);
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