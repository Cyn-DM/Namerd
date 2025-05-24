using Namerd.Domain.Results;
using Namerd.Persistence.Repository;

namespace Namerd.Services;

public class NominationService
{
    private readonly NominationRepository _nominationRepository;


    public NominationService(NominationRepository nominationRepository)
    {
        _nominationRepository = nominationRepository;
    }

    public Task<UserAlreadyNominatedResult> CheckIfUserAlreadyNominated(ulong guildId, ulong userId)
    {
        return _nominationRepository.CheckIfUserAlreadyNominated(guildId, userId);
    }
}