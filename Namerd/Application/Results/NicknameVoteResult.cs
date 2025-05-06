using Namerd.Domain.Enums;
using NetCord.Rest;

namespace Namerd.Application.Results;

public class NicknameVoteResult
{
    public InteractionMessageProperties MessageProperties { get;}
    public NicknameVoteErrorType ErrorType { get;}
    public bool IsSuccessful => ErrorType == NicknameVoteErrorType.None;
    
    private NicknameVoteResult(InteractionMessageProperties messageProperties, NicknameVoteErrorType errorType)
    {
        MessageProperties = messageProperties;
        ErrorType = errorType;
    }
    
    public static NicknameVoteResult Success(InteractionMessageProperties messageProperties)
    => new NicknameVoteResult(messageProperties, NicknameVoteErrorType.None);
    
    public static NicknameVoteResult Failure(InteractionMessageProperties messageProperties, NicknameVoteErrorType errorType)
    => new NicknameVoteResult(messageProperties, errorType);
}