using System.Collections.Generic;
using Model;

namespace Persistence
{

    public interface IRegistrationRepository : IRepository<long, Registration>
    {
        int getNrOfParticipants(Competition competition);

        IEnumerable<Participant> getParticipantsForCompetition(long competitionId);
    }
}