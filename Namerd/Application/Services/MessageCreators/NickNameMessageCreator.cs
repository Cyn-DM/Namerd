using Namerd.Application.Interfaces;
using NetCord;
using NetCord.Rest;
using NetCord.Services;

namespace Namerd.Application.Services.MessageCreators;

public static class NickNameMessageCreator
{
    public static async Task CreateVoteResultMessage(IInteractionContextWrapper contextWrapper, IGuildUserWrapper userWrapper,
        string nickname, bool voteSucceeded, bool userIsOwner)
    {
        var usernames = GetUsernames(contextWrapper, userWrapper);

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

        await contextWrapper.InteractionWrapper.Channel.SendMessageAsync(messageProperties);
    }

    public static InteractionMessageProperties CreateVoteStartMessage(IApplicationCommandContextWrapper contextWrapper, IGuildUserWrapper userWrapper, string nickname, int timeInMinutes)
    {
        var usernames = GetUsernames(contextWrapper, userWrapper);
        
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

    public static async Task CreateMentionMessage(IInteractionContextWrapper contextWrapper, IGuildUserWrapper userWrapper)
    {
        string messageContent = $"Calling {userWrapper}!";
        await contextWrapper.InteractionWrapper.Channel.SendMessageAsync(messageContent);
    }

    public static InteractionMessageProperties CreateInvalidNickNameMessage(string nickname)
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

    private static (string voteStarterName, string changingUserName) GetUsernames(IInteractionContextWrapper contextWrapper, IGuildUserWrapper userWrapper)
    {
        var voteStarterName = contextWrapper.InteractionWrapper.UserWrapper.Username;
        
        if (contextWrapper.InteractionWrapper.UserWrapper is IGuildUserWrapper guildUser)
        {
            var voteStarterNickname = guildUser.Nickname;

            if (voteStarterNickname != null)
            {
                voteStarterName = voteStarterNickname;
            }
        }

        var changingUserName = userWrapper.Username;
        
        if (userWrapper.Nickname != null)
        {
            changingUserName = userWrapper.Nickname;
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