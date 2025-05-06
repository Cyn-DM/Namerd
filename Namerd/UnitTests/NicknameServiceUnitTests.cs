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
    private IApplicationCommandContext _context;
    
    [SetUp]
    public void Setup()
    {
        _context = new Mock<IApplicationCommandContext>().Object;
        

    }
    
    [Test]
    public void VoteForNickname_NicknameInvalid_ReturnsError()
    {
        var result = NicknameService.VoteForNickname(_context, new Mock<IGuildUser>().Object, "a", 10);
        
        Assert.That(result.ErrorType, Is.EqualTo(NicknameVoteErrorType.NicknameInvalid));
        Assert.That(result.IsSuccessful, Is.False);
        Assert.That(result.MessageProperties, Is.Not.Null);
    }
}