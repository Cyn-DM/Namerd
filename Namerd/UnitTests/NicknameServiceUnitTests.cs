using Moq;
using Namerd.Application.Interfaces;
using Namerd.Application.Services;
using Namerd.Domain.Enums;
using NetCord;
using NetCord.Gateway;
using NUnit.Framework;

namespace Namerd.UnitTests;

[TestFixture]
public class NicknameServiceUnitTests
{
    private IApplicationCommandContextWrapper _contextWrapper;
    
    [SetUp]
    public void Setup()
    {
        var mockUser = new Mock<IUserWrapper>();
        mockUser.Setup(u => u.Username).Returns("TestUser");

        var mockInteraction = new Mock<IInteractionWrapper>();
        mockInteraction.Setup(i => i.UserWrapper).Returns(mockUser.Object);

        var mockContext = new Mock<IApplicationCommandContextWrapper>();
        mockContext.Setup(c => c.InteractionWrapper).Returns(mockInteraction.Object);

        _contextWrapper = mockContext.Object;
    }
    
    [Test]
    public void VoteForNickname_NicknameInvalid_ReturnsError()
    {
        var result = NicknameService.VoteForNickname(_contextWrapper, new Mock<IGuildUserWrapper>().Object, "a", 10);
        
        Assert.That(result.ErrorType, Is.EqualTo(NicknameVoteErrorType.NicknameInvalid));
        Assert.That(result.IsSuccessful, Is.False);
        Assert.That(result.MessageProperties, Is.Not.Null);
    }
    
    [Test]
    public void VoteForNickname_TimeInvalid_ReturnsError()
    {
        var result = NicknameService.VoteForNickname(_contextWrapper, new Mock<IGuildUserWrapper>().Object, "ValidNickname", -1);
        
        Assert.That(result.ErrorType, Is.EqualTo(NicknameVoteErrorType.TimeInvalid));
        Assert.That(result.IsSuccessful, Is.False);
        Assert.That(result.MessageProperties, Is.Not.Null);
    }
    
    [Test]
    public void VoteForNickname_ValidInput_ReturnsSuccess()
    {
        var result = NicknameService.VoteForNickname(_contextWrapper, new Mock<IGuildUserWrapper>().Object, "ValidNickname", 10);
        
        Assert.That(result.ErrorType, Is.EqualTo(NicknameVoteErrorType.None));
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.MessageProperties, Is.Not.Null);
    }
}