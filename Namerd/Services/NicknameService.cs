using Namerd.CustomExceptions;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Namerd.Services;

public class NicknameService
{
    
    
    public static async Task ChangeNickname(ApplicationCommandContext context, GuildUser user, string nickname)
    {
        var guild = context.Interaction.Guild ?? await context.Client.Rest.GetGuildAsync(context.Interaction.GuildId.Value);
            
        if (user.Id == guild.OwnerId)
        {
            throw new UserIsOwnerException();
        }
            
        await user.ModifyAsync(x => x.Nickname = nickname);
    }

    public static async Task VoteForNickName(ApplicationCommandContext context, GuildUser user, string nickname)
    {
        var message = await CreateVoteStartMessage(context, user, nickname);
        
        await Task.Delay(30000);
        
        var updatedMessage = await context.Channel.GetMessageAsync(message.Id);
        
        var voteSuceeded = CheckVoteSuccess(updatedMessage);

        if (voteSuceeded)
        {
            await ChangeNickname(context, user, nickname);
        }

        await CreateVoteResultMessage(context, user, nickname, voteSuceeded);
    }

    private static (string voteStarterName, string changingUserName) GetUsernames(ApplicationCommandContext context, GuildUser user)
    {
        var voteStarterName = context.User.Username;
        
        if (context.Interaction.User is GuildInteractionUser guildUser)
        {
            var voteStarterNickname = guildUser.Nickname;

            if (voteStarterNickname != null)
            {
                voteStarterName = voteStarterNickname;
            }
        }

        var changingUserName = user.Username;
        
        if (user.Nickname != null)
        {
            changingUserName = user.Nickname;
        }
        
        return (voteStarterName, changingUserName);
    }

    private static async Task<RestMessage> CreateVoteStartMessage(ApplicationCommandContext context, GuildUser user, string nickname)
    {
        var usernames = GetUsernames(context, user);
        
        var embed = new EmbedProperties()
            .WithTitle($"New nickname vote for {usernames.changingUserName}!")
            .WithDescription($"{usernames.voteStarterName} wants to start a vote to change {usernames.changingUserName}'s nickname.")
            .AddFields(
                new EmbedFieldProperties().WithName("\u200B").WithValue("\u200B"),
                new EmbedFieldProperties().WithName("The nickname:").WithValue(nickname),
                new EmbedFieldProperties().WithName("\u200B").WithValue("\u200B"),
                new EmbedFieldProperties().WithValue("Please vote by choosing a reaction on the message.")
                )
            .WithColor(new (0xE8004F));
        
        
        var messageProperties = new MessageProperties
        {
            Embeds = [embed]
        };

        var message = await context.Channel.SendMessageAsync(messageProperties);
        await message.AddReactionAsync("👍");
        await message.AddReactionAsync("👎");

        return message;
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

        if (yesCount > 1 && yesCount > noCount)
        {
            return true;
        }

        return false;
    }

    private static async Task<RestMessage> CreateVoteResultMessage(ApplicationCommandContext context, GuildUser user,
        string nickname, bool voteSucceeded)
    {
        var usernames = GetUsernames(context, user);

        var result = "";
        if (voteSucceeded)
        {
            result = "The vote succeeded and the nickname has been changed!";
        }
        else
        {
            result = "The vote failed!";
        }
        
        
        var embed = new EmbedProperties()
            .WithTitle($"Voting completed for {usernames.changingUserName}!")
            .WithDescription(result)
            .AddFields(
                new EmbedFieldProperties().WithName("\u200B").WithValue("\u200B"),
                new EmbedFieldProperties().WithName("The nickname:").WithValue(nickname)
            )
            .WithColor(new (0xE8004F));
        
        
        var messageProperties = new MessageProperties
        {
            Embeds = [embed]
        };

        var message = await context.Channel.SendMessageAsync(messageProperties);

        return message;
    }
}