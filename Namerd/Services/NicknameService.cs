using Namerd.CustomExceptions;
using NetCord;
using NetCord.Services.ApplicationCommands;

namespace Namerd.Services;

public class NicknameService
{
    
    public static async Task ChangeNickname(ApplicationCommandContext context, GuildUser user, string nickname)
    {
        var guild = context.Interaction.Guild ?? await context.Client.Rest.GetGuildAsync(context.Interaction.GuildId.Value);
            
        if (user.Id == guild.OwnerId)
        {
            throw new UserIsOwnerException();
        }
            
        await user.ModifyAsync(x => x.Nickname = nickname);
    }

    public static async Task VoteForNickName()
    {
        //Respond and create vote message 
        
        //Wait for vote period to be over
        
        // Check votes
        
        // Change nickname if enough
        
        //Respond with results
        
    }
    
    private 
}