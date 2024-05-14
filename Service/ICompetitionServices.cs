using System;
using System.Collections.Generic;
using Model;

namespace Service
{
    public interface ICompetitionServices
    {
        User Connect(string username, string password, ICompetitionObserver client);

        void Logout(string username);

        void RegisterParticipant(string name, DateTime birthDate, string proba);

        int GetNrOfParticipants(Competition competition);

        IEnumerable<User> GetAllUsers();

        IEnumerable<Competition> GetAllCompetitions();

        IterableDTO GetParticipantsForCompetition(long competitionId);
    }
}