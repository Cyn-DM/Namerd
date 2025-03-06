using NetCord;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;

namespace Namerd.Services.MessageCreators;

public static class NickNameMessageCreator
{
    public static async Task CreateVoteResultMessage(IInteractionContext context, GuildUser user,
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

        await context.Interaction.Channel.SendMessageAsync(messageProperties);
    }

    public static InteractionMessageProperties CreateVoteStartMessage(ApplicationCommandContext context, GuildUser user, string nickname, int timeInMinutes)
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
        
        
        var messageProperties = new InteractionMessageProperties()
        {
            Embeds = [embed],
        };

        return messageProperties;
    }

    public static async Task CreateMentionMessage(IInteractionContext context, GuildUser user)
    {
        string messageContent = $"Calling {user}!";
        await context.Interaction.Channel.SendMessageAsync(messageContent);
    }

    public static InteractionMessageProperties CreateInvalidNickNameMessage(ApplicationCommandContext context, string nickname)
    {
        var embed = new EmbedProperties()
            .WithTitle($"Sorry, {nickname} is not a valid nickname.")
            .WithColor(new (0xE8004F));
        
        var messageProperties = new InteractionMessageProperties()
        {
            Embeds = [embed]
        };

        return messageProperties;
    }

    private static (string voteStarterName, string changingUserName) GetUsernames(IInteractionContext context, GuildUser user)
    {
        var voteStarterName = context.Interaction.User.Username;
        
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

    public static InteractionMessageProperties CreateInvalidTimeMessage()
    {
        var embed = new EmbedProperties()
            .WithTitle($"Please choose a time between 1 minute and 1440 minutes (24 hours).")
            .WithColor(new (0xE8004F));
        
        var messageProperties = new InteractionMessageProperties()
        {
            Embeds = [embed]
        };

        return messageProperties;
    }
}