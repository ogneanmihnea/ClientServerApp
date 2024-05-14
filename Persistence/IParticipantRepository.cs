using System;
using Model;

namespace Persistence
{

    public interface IParticipantRepository : IRepository<long, Participant>
    {
        Participant findByName(String name);
    }
}