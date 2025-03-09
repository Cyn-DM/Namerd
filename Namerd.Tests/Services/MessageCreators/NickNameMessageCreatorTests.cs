using Moq;
using Namerd.Services.MessageCreators;
using NetCord;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;
using NUnit.Framework;

namespace Namerd.Tests.Services.MessageCreators;

[TestFixture]
public class NickNameMessageCreatorTests
{
    private Mock<ApplicationCommandContext> _contextMock;
    private Mock<GuildUser> _userMock;
    private Mock<GuildInteractionUser> _interactionUserMock;
    private Mock<ApplicationCommandInteraction> _interactionMock;

    [SetUp]
    public void Setup()
    {
        _contextMock = new Mock<ApplicationCommandContext>();
        _userMock = new Mock<GuildUser>();
        _interactionUserMock = new Mock<GuildInteractionUser>();
        _interactionMock = new Mock<ApplicationCommandInteraction>();

        _interactionMock.SetupGet(i => i.User).Returns(_interactionUserMock.Object);
        _contextMock.SetupGet(c => c.Interaction).Returns(_interactionMock.Object);
    }

    [Test]
    public void CreateVoteStartMessage_SetsCorrectProperties()
    {
        // Arrange
        const string nickname = "TestNick";
        const int timeInMinutes = 5;
        _interactionUserMock.SetupGet(u => u.Username).Returns("VoteStarter");
        _userMock.SetupGet(u => u.Username).Returns("TargetUser");

        // Act
        var result = NickNameMessageCreator.CreateVoteStartMessage(_contextMock.Object, _userMock.Object, nickname, timeInMinutes);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Embeds, Is.Not.Empty);
        var embed = result.Embeds.First();
        Assert.That(embed.Title, Does.Contain("New nickname vote"));
        Assert.That(embed.Description, Does.Contain("VoteStarter wants to start a vote"));
        Assert.That(embed.Fields.Count(), Is.EqualTo(4));
        Assert.That(embed.Fields.ElementAt(1).Name, Is.EqualTo("The nickname:"));
        Assert.That(embed.Fields.ElementAt(1).Value, Is.EqualTo(nickname));
        Assert.That(embed.Fields.ElementAt(3).Value, Does.Contain("5 minutes"));
        Assert.That(embed.Color, Is.EqualTo(new Color(0xE8004F)));
    }

    [Test]
    public void CreateInvalidNickNameMessage_SetsCorrectProperties()
    {
        // Arrange
        const string invalidNickname = "@invalid@";

        // Act
        var result = NickNameMessageCreator.CreateInvalidNickNameMessage(_contextMock.Object, invalidNickname);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Embeds, Is.Not.Empty);
        var embed = result.Embeds.First();
        Assert.That(embed.Title, Does.Contain("not a valid nickname"));
        Assert.That(embed.Color, Is.EqualTo(new Color(0xE8004F)));
    }

    [Test]
    public void CreateInvalidTimeMessage_SetsCorrectProperties()
    {
        // Act
        var result = NickNameMessageCreator.CreateInvalidTimeMessage();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Embeds, Is.Not.Empty);
        var embed = result.Embeds.First();
        Assert.That(embed.Title, Does.Contain("choose a time between"));
        Assert.That(embed.Color, Is.EqualTo(new Color(0xE8004F)));
    }

    [Test]
    public async Task CreateVoteResultMessage_Success_SetsCorrectProperties()
    {
        // Arrange
        const string nickname = "TestNick";
        _interactionUserMock.SetupGet(u => u.Username).Returns("VoteStarter");
        _userMock.SetupGet(u => u.Username).Returns("TargetUser");

        // Act
        await NickNameMessageCreator.CreateVoteResultMessage(_contextMock.Object, _userMock.Object, nickname, true, false);

        // Assert
        _contextMock.Verify(
            c => c.Interaction.Channel.SendMessageAsync(
                It.Is<MessageProperties>(m => 
                    m.Embeds.First().Title.Contains("Voting completed") &&
                    m.Embeds.First().Description.Contains("succeeded") &&
                    m.Embeds.First().Description.Contains("nickname has been changed") &&
                    m.Embeds.First().Color.Equals(new Color(0xE8004F))
                ),
                null,  // RestRequestProperties parameter
                CancellationToken.None),  // CancellationToken parameter
            Times.Once);
    }

    [Test]
    public async Task CreateVoteResultMessage_FailedVote_SetsCorrectProperties()
    {
        // Arrange
        const string nickname = "TestNick";
        _interactionUserMock.SetupGet(u => u.Username).Returns("VoteStarter");
        _userMock.SetupGet(u => u.Username).Returns("TargetUser");

        // Act
        await NickNameMessageCreator.CreateVoteResultMessage(_contextMock.Object, _userMock.Object, nickname, false, false);

        // Assert
        _contextMock.Verify(
            c => c.Interaction.Channel.SendMessageAsync(
                It.Is<MessageProperties>(m => 
                    m.Embeds.First().Title.Contains("Voting completed") &&
                    m.Embeds.First().Description.Contains("failed") &&
                    m.Embeds.First().Color.Equals(new Color(0xE8004F))
                ),
                null,  // RestRequestProperties parameter
                CancellationToken.None),  // CancellationToken parameter
            Times.Once);
    }

    [Test]
    public async Task CreateVoteResultMessage_SuccessButOwner_SetsCorrectProperties()
    {
        // Arrange
        const string nickname = "TestNick";
        _interactionUserMock.SetupGet(u => u.Username).Returns("VoteStarter");
        _userMock.SetupGet(u => u.Username).Returns("TargetUser");

        // Act
        await NickNameMessageCreator.CreateVoteResultMessage(_contextMock.Object, _userMock.Object, nickname, true, true);

        // Assert
        _contextMock.Verify(
            c => c.Interaction.Channel.SendMessageAsync(
                It.Is<MessageProperties>(m => 
                    m.Embeds.First().Title.Contains("Voting completed") &&
                    m.Embeds.First().Description.Contains("succeeded") &&
                    m.Embeds.First().Description.Contains("permission issues") &&
                    m.Embeds.First().Color.Equals(new Color(0xE8004F))
                ),
                null,  // RestRequestProperties parameter
                CancellationToken.None),  // CancellationToken parameter
            Times.Once);
    }
} 