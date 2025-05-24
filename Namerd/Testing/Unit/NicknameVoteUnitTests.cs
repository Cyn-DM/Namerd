using Namerd.Domain;
using NUnit.Framework;

namespace Namerd.Testing.Unit;

[TestFixture]
public class NicknameVoteUnitTests
{
    [TestCase("ValidNick")]
    [TestCase("EmojiNickname 😃")] //Nickname with an emoji
    [Test] public void IsValidNickname_ValidNickname_ReturnsTrue(string validNickname)
    {
        // Arrange
        var vote = new NicknameVote(validNickname, 60);
        
        // Act
        var result = vote.IsValidNickname();
        
        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    [TestCase("a")] // Too short
    [TestCase("ThisNicknameIsWayTooLongForDiscordToHandle")] // Too long
    [TestCase("everyone")] // Reserved word
    [TestCase("here")] // Reserved word
    [TestCase("")] // Empty
    public void IsValidNickname_InvalidNickname_ReturnsFalse(string invalidNickname)
    {
        // Arrange
        var vote = new NicknameVote(invalidNickname, 60);
        
        // Act
        var result = vote.IsValidNickname();
        
        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    [TestCase(1)]
    [TestCase(60)]
    [TestCase(1440)]
    public void IsValidTime_ValidTime_ReturnsTrue(int validTime)
    {
        // Arrange
        var vote = new NicknameVote("ValidNick", validTime);
        
        // Act
        var result = vote.IsValidTime();
        
        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(1441)]
    public void IsValidTime_InvalidTime_ReturnsFalse(int invalidTime)
    {
        // Arrange
        var vote = new NicknameVote("ValidNick", invalidTime);
        
        // Act
        var result = vote.IsValidTime();
        
        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    [TestCase(5, 3, true)]
    [TestCase(3, 5, false)]
    [TestCase(5, 5, false)]
    public void IsValidVoteResult_ValidatesCorrectly(int yesCount, int noCount, bool expectedResult)
    {
        // Act
        var result = NicknameVote.IsValidVoteResult(yesCount, noCount);
        
        // Assert
        Assert.That(result, Is.EqualTo(expectedResult));
    }
}