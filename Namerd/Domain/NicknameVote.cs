using System.Text.RegularExpressions;

namespace Namerd.Domain;

public class NicknameVote
{
    private static readonly Regex NicknameRegex = new Regex(@"^(?=.{2,32}$)(?!(?:everyone|here)$).+$", RegexOptions.Singleline);
    public string Nickname { get; }
    public int TimeInMinutes { get; }


    public NicknameVote(string nickname, int timeInMinutes)
    {
        Nickname = nickname;
        TimeInMinutes = timeInMinutes;
    }
    
    public bool IsValidNickname()
    {
        return NicknameRegex.IsMatch(Nickname);
    }
    
    public bool IsValidTime()
    {
        return TimeInMinutes > 0 && TimeInMinutes <= 1440;
    }

    public static bool IsValidVoteResult(int yesCount, int noCount)
    {
        return yesCount > noCount;
    }
}