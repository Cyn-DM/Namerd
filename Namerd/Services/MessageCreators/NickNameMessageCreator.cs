using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Namerd.Services.MessageCreators;

public static class NickNameMessageCreator
{
    public static async Task<RestMessage> CreateVoteResultMessage(ApplicationCommandContext context, GuildUser user,
        string nickname, bool voteSucceeded, bool userIsOwner)
    {
        var usernames = GetUsernames(context, user);

        var result = "";
        if (voteSucceeded && !userIsOwner)
        {
            result = $"The vote succeeded and the nickname has been changed! I dub thee {nickname}.";
        } else if (voteSucceeded && userIsOwner)
        {
            result = "The vote succeeded, " +
                     "but the nickname has not been changed because of permission issues. " +
                     "Please change the nickname yourself.";
        }
        else
        {
            result = "The vote failed!";
        }
        
        
        var embed = new EmbedProperties()
            .WithTitle($"Voting completed for {usernames.changingUserName}!")
            .WithDescription(result)
            .WithColor(new (0xE8004F));
        
        
        var messageProperties = new MessageProperties
        {
            Embeds = [embed]
        };

        var message = await context.Channel.SendMessageAsync(messageProperties);

        return message;
    }

    public static async Task<RestMessage> CreateVoteStartMessage(ApplicationCommandContext context, GuildUser user, string nickname, int timeInMinutes)
    {
        var usernames = GetUsernames(context, user);
        
        var embed = new EmbedProperties()
            .WithTitle($"New nickname vote for {usernames.changingUserName}!")
            .WithDescription($"{usernames.voteStarterName} wants to start a vote to change {usernames.changingUserName}'s nickname.")
            .AddFields(
                new EmbedFieldProperties().WithName("\u200B").WithValue("\u200B"),
                new EmbedFieldProperties().WithName("The nickname:").WithValue(nickname),
                new EmbedFieldProperties().WithName("\u200B").WithValue("\u200B"),
                new EmbedFieldProperties().WithValue($"Please vote by choosing a reaction on the message. The vote ends in {timeInMinutes} minutes.")
            )
            .WithColor(new (0xE8004F));
        
        
        var messageProperties = new MessageProperties
        {
            Embeds = [embed]
        };


        await CreateMentionMessage(context, user);
        var message = await context.Channel.SendMessageAsync(messageProperties);
        await message.AddReactionAsync("👍");
        await message.AddReactionAsync("👎");

        return message;
    }

    private static async Task CreateMentionMessage(ApplicationCommandContext context, GuildUser user)
    {
        string messageContent = $"Calling {user}!"; // 'user.ToString()' returns the mention string
        await context.Channel.SendMessageAsync(messageContent);
    }

    public static async Task CreateInvalidNickNameMessage(ApplicationCommandContext context, string nickname)
    {
        var embed = new EmbedProperties()
            .WithTitle($"Sorry, {nickname} is not a valid nickname.")
            .WithColor(new (0xE8004F));
        
        var messageProperties = new MessageProperties
        {
            Embeds = [embed]
        };

        await context.Channel.SendMessageAsync(messageProperties);
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

    public static async Task CreateInvalidTimeMessage(ApplicationCommandContext context)
    {
        var embed = new EmbedProperties()
            .WithTitle($"Please choose a time between 1 minute and 1440 minutes (24 hours).")
            .WithColor(new (0xE8004F));
        
        var messageProperties = new MessageProperties
        {
            Embeds = [embed]
        };

        await context.Channel.SendMessageAsync(messageProperties);
    }
}