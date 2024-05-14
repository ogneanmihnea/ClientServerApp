using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;
using Persistence;
using Service;

namespace Server
{
    public class CompetitionServices:ICompetitionServices
    {
        private IUserRepository _userRepo;
        private ICompetitionRepository _competitionRepo;
        private IParticipantRepository _participantRepo;
        private IRegistrationRepository _registrationRepo;
        private ConcurrentDictionary<string, ICompetitionObserver> _loggedClients;
        private readonly int defaultThreads = 5;
        private readonly object _lock = new object();
        
        
        public CompetitionServices(IUserRepository userRepo, ICompetitionRepository competitionRepo, IParticipantRepository participantRepo, IRegistrationRepository registrationRepo)
        {
            this._userRepo = userRepo;
            this._competitionRepo = competitionRepo;
            this._participantRepo = participantRepo;
            this._registrationRepo = registrationRepo;
            this._loggedClients = new ConcurrentDictionary<string, ICompetitionObserver>();
            
        }
        
        
        public User Connect(string username, string password,ICompetitionObserver client)
        {
            lock (_lock)
            {
                User user = _userRepo.FindByUsername(username);
                if (user != null && user.password.Equals(password))
                {
                    if (_loggedClients.ContainsKey(username))
                    {
                        throw new Exception("User already logged in");
                    }
                    else
                    {
                        _loggedClients[username] = client;
                        return user;
                    }
                }
                return null;
            }
        }

        private bool CheckCompetitionTrial(string proba)
        {
            IEnumerable<Competition> competitions = _competitionRepo.FindAll();
            return competitions.Any(c => c.trial == proba);
        }
        
        private bool CheckCompetitionAgeInterval(int age)
        {
            IEnumerable<Competition> competitions = _competitionRepo.FindAll();
            return competitions.Any(c => c.startingAgeInterval <= age && c.endingAgeInterval >= age);
        }
        
        private Competition FindByTrialAndAge(string proba, int age)
        {
            IEnumerable<Competition> competitions = _competitionRepo.FindAll();
            return competitions.First(c => c.trial == proba && c.startingAgeInterval <= age && c.endingAgeInterval >= age);
        }
        
        public void RegisterParticipant(string name, DateTime birthDate, string proba)
        {
            Participant participant = new Participant(name, birthDate);
            if (!CheckCompetitionTrial(proba))
            {
                throw new Exception("The competition does not exist!");
            }

            int age = DateTime.Now.Year - birthDate.Year;
            if (!CheckCompetitionAgeInterval(age))
            {
                throw new Exception("The participant does not fit the age interval of the competition!");
            }

            Competition competition = FindByTrialAndAge(proba,age);

            _participantRepo.save(participant);
            participant.Id = _participantRepo.findByName(name).Id;
            Registration registration = new Registration(participant, competition);
            _registrationRepo.save(registration);

            NotifyAllClients();
        }

        public int GetNrOfParticipants(Competition competition)
        {
            return _registrationRepo.getNrOfParticipants(competition);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepo.FindAll();
        }

        public IEnumerable<Competition> GetAllCompetitions()
        {
            return _competitionRepo.FindAll();
        }

        public IterableDTO GetParticipantsForCompetition(long competitionId)
        {
            IterableDTO iterableDTO = new IterableDTO(_registrationRepo.getParticipantsForCompetition(competitionId));
            return iterableDTO;
        }


        private void NotifyAllClients()
        {
            Console.WriteLine($"Notifying clients {_loggedClients.Count}");
            foreach (var client in _loggedClients.Values)
            {
                Task.Run(() => client.registerParticipant());
            }
        }
        
        public void Logout(string username)
        {
            lock (_lock)
            {
                bool removed = _loggedClients.TryRemove(username, out ICompetitionObserver removedClient);
                if (removed)
                {
                    Console.WriteLine("Client " + username + " logged out");
                }
            }
        }
        
        // [Serializable]
        // private class CompetitionComparator : IComparer<Competition>
        // {
        //     public int Compare(Competition c1, Competition c2)
        //     {
        //         int distanceComparison = c1.Id.CompareTo(c2.Id);
        //         if (distanceComparison != 0)
        //         {
        //             return distanceComparison;
        //         }
        //         else
        //         {
        //             return string.Compare(c1.Style, c2.Style, StringComparison.Ordinal);
        //         }
        //     }
        // }
    }
}
