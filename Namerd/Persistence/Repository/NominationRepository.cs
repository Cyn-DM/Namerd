using Microsoft.EntityFrameworkCore;
using Namerd.CustomExceptions;
using Namerd.Domain.Results;
using Namerd.Persistence.Context;

namespace Namerd.Persistence.Repository;

public class NominationRepository
{
    private readonly NamerdContext _context;

    public NominationRepository(NamerdContext context)
    {
        _context = context;
    }
    
    public async Task<UserAlreadyNominatedResult> CheckIfUserAlreadyNominated(ulong guildId, ulong userId)
    {
        // Check if the bot is set up
        var now = DateTime.UtcNow;

        var botCheck = await _context.Bots
            .Where(n => n.GuildId == guildId)
            .Select(n => new
                {
                    DoesNominationPeriodExist = n.NominationPeriods.Any(np => np.StartDateTime <= now && np.EndDateTime > now)
                }
            )
            .FirstOrDefaultAsync();
            
        if (botCheck == null)
        {
            return UserAlreadyNominatedResult.Failure("Please set up the bot first.");
        }
        
        if (!botCheck.DoesNominationPeriodExist)
        {
            throw new NominationPeriodMissingException();
        }
        
        // Check if the user is already nominated
        var result = await _context.Bots
            .Where(n => n.GuildId == guildId)
            .Select(n => n.NominationPeriods
                .Where(np => np.StartDateTime <= now && np.EndDateTime > now)
                .SelectMany(np => np.Nominations)
                .Any(nd => nd.UserId == userId))
            .FirstOrDefaultAsync();
        
        if (result)
        {
            return UserAlreadyNominatedResult.Nominated();
        }
        
        return UserAlreadyNominatedResult.NotNominated();
    }
}