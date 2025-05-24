using System.Text.RegularExpressions;
using Namerd.CustomExceptions;
using Namerd.Domain;
using Namerd.Services.MessageCreators;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.Commands;

namespace Namerd.Services;

public static partial class NicknameService
{
    public static InteractionMessageProperties VoteForNickName(ApplicationCommandContext context, GuildUser user, string nickname, int timeInMinutes)
    {
        var vote = new NicknameVote(nickname, timeInMinutes);
        
        if (!vote.IsValidNickname())
        {
            return NickNameMessageCreator.CreateInvalidNickNameMessage(context, nickname);
        }

        if (!vote.IsValidTime())
        {
            return NickNameMessageCreator.CreateInvalidTimeMessage();
        }
        
        var messageProperties =  NickNameMessageCreator.CreateVoteStartMessage(context, user, nickname, timeInMinutes);
        
        
        return messageProperties;
    }

    public static async Task MentionUserAsync(IInteractionContext context, GuildUser user)
    {
        await NickNameMessageCreator.CreateMentionMessage(context, user);
    }

    public static async Task ProcessVoting(ulong messageId, int timeInMinutes, IInteractionContext context, GuildUser user, string nickname)
    {
        var milliseconds = timeInMinutes * 60 * 1000;
        
        await Task.Delay(milliseconds);
        
        var updatedMessage = await context.Interaction.Channel.GetMessageAsync(messageId);
        var voteCount = CountVotes(updatedMessage);
        var voteSucceeded = NicknameVote.IsValidVoteResult(voteCount.yesCount, voteCount.noCount);
        
        try
        {
            if (voteSucceeded)
            {
                await ChangeNickname(context, user, nickname);
            }

            await NickNameMessageCreator.CreateVoteResultMessage(context, user, nickname, voteSucceeded, false);
        }
        catch (UserIsOwnerException)
        {
            await NickNameMessageCreator.CreateVoteResultMessage(context, user, nickname, voteSucceeded, true);
        }
    }

    private static async Task ChangeNickname(IInteractionContext context, GuildUser user, string nickname)
    {
        var guild = context.Interaction.Guild;
            
        if (user.Id == guild.OwnerId)
        {
            throw new UserIsOwnerException();
        }
            
        await user.ModifyAsync(x => x.Nickname = nickname);
    }

    private static (int yesCount, int noCount) CountVotes(RestMessage restMessage)
    {
        var reactions = restMessage.Reactions;
        
        int yesCount = 0;
        int noCount = 0;

        foreach (var reaction in reactions)
        {
            if (reaction.Emoji.Name == "👍")
            {
                yesCount = reaction.Count - 1;
            } else if (reaction.Emoji.Name == "👎")
            {
                noCount = reaction.Count - 1;
            }
        }

        return (yesCount, noCount);
    }
    
}