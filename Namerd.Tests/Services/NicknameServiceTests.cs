using Moq;
using Namerd.CustomExceptions;
using Namerd.Services;
using NetCord;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;
using NUnit.Framework;
using NetCord.Gateway;
using NetCord.JsonModels;

namespace Namerd.Tests.Services;

[TestFixture]
public class NicknameServiceTests
{
    private Mock<ApplicationCommandContext> _contextMock;
    private Mock<GuildUser> _userMock;
    private Mock<Guild> _guildMock;
    private Mock<GuildInteractionUser> _interactionUserMock;
    private Mock<ApplicationCommandInteraction> _interactionMock;
    private Mock<TextChannel> _channelMock;

    [SetUp]
    public void Setup()
    {
        _contextMock = new Mock<ApplicationCommandContext>();
        _userMock = new Mock<GuildUser>();
        _guildMock = new Mock<Guild>();
        _interactionUserMock = new Mock<GuildInteractionUser>();
        _interactionMock = new Mock<ApplicationCommandInteraction>();
        _channelMock = new Mock<TextChannel>();

        _interactionMock.SetupGet(i => i.User).Returns(_interactionUserMock.Object);
        _interactionMock.SetupGet(i => i.Guild).Returns(_guildMock.Object);
        _interactionMock.SetupGet(i => i.Channel).Returns(_channelMock.Object);
        _contextMock.SetupGet(c => c.Interaction).Returns(_interactionMock.Object);
    }

    [Test]
    public void VoteForNickName_WithValidNickname_ReturnsVoteMessage()
    {
        // Arrange
        const string nickname = "ValidNick";
        const int timeInMinutes = 5;

        // Act
        var result = NicknameService.VoteForNickName(_contextMock.Object, _userMock.Object, nickname, timeInMinutes);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Embeds, Is.Not.Empty);
        var embed = result.Embeds.First();
        Assert.That(embed.Title, Does.Contain("New nickname vote"));
        Assert.That(embed.Color, Is.EqualTo(new Color(0xE8004F)));
    }

    [Test]
    public void VoteForNickName_WithInvalidNickname_ReturnsErrorMessage()
    {
        // Arrange
        const string nickname = "@invalid@";
        const int timeInMinutes = 5;

        // Act
        var result = NicknameService.VoteForNickName(_contextMock.Object, _userMock.Object, nickname, timeInMinutes);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Embeds, Is.Not.Empty);
        var embed = result.Embeds.First();
        Assert.That(embed.Title, Does.Contain("not a valid nickname"));
    }

    [Test]
    public void VoteForNickName_WithInvalidTime_ReturnsErrorMessage()
    {
        // Arrange
        const string nickname = "ValidNick";
        const int timeInMinutes = 0;

        // Act
        var result = NicknameService.VoteForNickName(_contextMock.Object, _userMock.Object, nickname, timeInMinutes);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Embeds, Is.Not.Empty);
        var embed = result.Embeds.First();
        Assert.That(embed.Title, Does.Contain("choose a time between"));
    }

    [Test]
    public async Task ChangeNickname_WhenUserIsOwner_ThrowsUserIsOwnerException()
    {
        // Arrange
        const string nickname = "NewNick";
        const ulong ownerId = 123456789;
        
        _guildMock.SetupGet(g => g.OwnerId).Returns(ownerId);
        _userMock.SetupGet(u => u.Id).Returns(ownerId);

        // Act & Assert
        await Task.Delay(1); // Ensure async context
        await NicknameService.ProcessVoting(1, 1, _contextMock.Object, _userMock.Object, nickname);
        
        _userMock.Verify(u => u.ModifyAsync(It.IsAny<Action<GuildUserOptions>>(), null, CancellationToken.None), Times.Never);
    }

    [Test]
    public async Task ProcessVoting_WhenVoteSucceeds_ChangesNickname()
    {
        // Arrange
        const string nickname = "NewNick";
        const ulong messageId = 123456789;
        var reactions = new List<MessageReaction>();

        // Create thumbs up reaction
        var thumbsUpJsonEmoji = new JsonEmoji
        {
            Name = "👍"
        };

        var thumbsUpReaction = new JsonMessageReaction
        {
            Emoji = thumbsUpJsonEmoji,
            Count = 3,
            Me = false
        };

        // Create thumbs down reaction
        var thumbsDownJsonEmoji = new JsonEmoji
        {
            Name = "👎"
        };

        var thumbsDownReaction = new JsonMessageReaction
        {
            Emoji = thumbsDownJsonEmoji,
            Count = 2,
            Me = false
        };

        // Create MessageReaction objects with the JsonMessageReaction objects
        reactions.Add(new MessageReaction(thumbsUpReaction));
        reactions.Add(new MessageReaction(thumbsDownReaction));

        var messageMock = new Mock<RestMessage>();
        messageMock.SetupGet(m => m.Reactions).Returns(reactions);

        // Fix the optional parameter issue with explicit parameters
        _channelMock.Setup(c => c.GetMessageAsync(messageId, null, CancellationToken.None))
            .ReturnsAsync(messageMock.Object);

        // Act
        await NicknameService.ProcessVoting(messageId, 1, _contextMock.Object, _userMock.Object, nickname);

        // Assert
        _userMock.Verify(
            u => u.ModifyAsync(
                It.IsAny<Action<GuildUserOptions>>(), 
                null, 
                CancellationToken.None),
            Times.Once);
    }
} 