using System.Text.RegularExpressions;
using Namerd.CustomExceptions;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Namerd.Services;

public static partial class NicknameService
{
    [GeneratedRegex(@"^(?=.{2,32}$)(?!(?:everyone|here)$).+$", RegexOptions.Singleline)]

    private static partial Regex NicknameRegex();
    
    public static async Task<bool> ValidateNickname(ApplicationCommandContext context, string nickname)
    {
        var result = NicknameRegex().Match(nickname);
        
        Console.WriteLine(nickname.Length);
        
        if (!result.Success)
        {
            await MessageCreator.CreateInvalidNickNameMessage(context, nickname);
        }
        
        return result.Success;
    }

    public static async Task VoteForNickName(ApplicationCommandContext context, GuildUser user, string nickname, int timeInMinutes)
    {
        var message = await MessageCreator.CreateVoteStartMessage(context, user, nickname, timeInMinutes);
        
        var milliseconds = timeInMinutes * 60 * 1000;
        
        await Task.Delay(milliseconds);
        
        var updatedMessage = await context.Channel.GetMessageAsync(message.Id);
        
        var voteSuceeded = CheckVoteSuccess(updatedMessage);

        try
        {
            if (voteSuceeded)
            {
                await ChangeNickname(context, user, nickname);
            }

            await MessageCreator.CreateVoteResultMessage(context, user, nickname, voteSuceeded, false);
        }
        catch (UserIsOwnerException)
        {
            await MessageCreator.CreateVoteResultMessage(context, user, nickname, voteSuceeded, true);
        }
        
    }

    private static async Task ChangeNickname(ApplicationCommandContext context, GuildUser user, string nickname)
    {
        var guild = context.Interaction.Guild ?? await context.Client.Rest.GetGuildAsync(context.Interaction.GuildId.Value);
            
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

    public static async Task<bool> CheckTime(ApplicationCommandContext context, int timeInMinutes)
    {
        if (timeInMinutes <= 0 || timeInMinutes > 1440)
        {
            await MessageCreator.CreateInvalidTimeMessage(context);
            return false;
        }

        return true;
    }

}