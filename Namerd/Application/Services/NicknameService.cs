using System.Text.RegularExpressions;
using Namerd.Application.Interfaces;
using Namerd.Application.Results;
using Namerd.Application.Services.MessageCreators;
using Namerd.CustomExceptions;
using Namerd.Domain.Enums;
using NetCord;
using NetCord.Rest;
using NetCord.Services;

namespace Namerd.Application.Services;

public static partial class NicknameService
{
    [GeneratedRegex(@"^(?=.{2,32}$)(?!(?:everyone|here)$).+$", RegexOptions.Singleline)]

    private static partial Regex NicknameRegex();
    

    public static NicknameVoteResult VoteForNickname(IApplicationCommandContext context, IGuildUser user, string nickname, int timeInMinutes)
    {
        var result = NicknameRegex().Match(nickname);
        InteractionMessageProperties properties;
        
        if (!result.Success)
        {
            properties = NickNameMessageCreator.CreateInvalidNickNameMessage(nickname);
            return NicknameVoteResult.Failure(properties,NicknameVoteErrorType.NicknameInvalid);
        }

        if (!CheckTimeCorrect(timeInMinutes))
        {
            properties = NickNameMessageCreator.CreateInvalidTimeMessage();
            return NicknameVoteResult.Failure(properties,NicknameVoteErrorType.TimeInvalid);
        }
        
        properties =  NickNameMessageCreator.CreateVoteStartMessage(context, user, nickname, timeInMinutes);
        
        return NicknameVoteResult.Success(properties);
    }

    public static async Task MentionUserAsync(IInteractionContext context, IGuildUser user)
    {
        await NickNameMessageCreator.CreateMentionMessage(context, user);
    }

    public static async Task ProcessVoting(ulong messageId, int timeInMinutes, IInteractionContext context, IGuildUser user, string nickname)
    {
        var milliseconds = timeInMinutes * 60 * 1000;
        
        await Task.Delay(milliseconds);
        
        var updatedMessage = await context.Interaction.Channel.GetMessageAsync(messageId);
        
        var voteSucceeded = CheckVoteSuccess(updatedMessage);

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

    private static async Task ChangeNickname(IInteractionContext context, IGuildUser user, string nickname)
    {
        var guild = context.Interaction.Guild;
            
        if (user.Id == guild.OwnerId)
        {
            throw new UserIsOwnerException();
        }
            
        await user.ModifyAsync(x => x.Nickname = nickname);
    }

    private static bool CheckVoteSuccess(RestMessage restMessage)
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

        if (yesCount > noCount)
        {
            return true;
        }

        return false;
    }

    private static bool CheckTimeCorrect(int timeInMinutes)
    {
        if (timeInMinutes <= 0 || timeInMinutes > 1440)
        {
            return false;
        }

        return true;
    }

}