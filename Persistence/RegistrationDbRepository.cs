using System;
using System.Collections.Generic;
using System.Data;
using log4net;
using Model;

namespace Persistence
{

    public class RegistrationDbRepository : IRegistrationRepository
    {
        private static readonly ILog log = LogManager.GetLogger("RegistrationDbRepository");

        IDictionary<String, string> Props;

        public RegistrationDbRepository(IDictionary<String, string> props)
        {
            log.Info("Creating RegistrationDBRepository");
            this.Props = props;
        }

        private Competition findComp(long id)
        {
            log.InfoFormat("Entering FindOne with value {0}", id);
            IDbConnection con = DbUtils.getConnection(Props);
            using (var comm = con.CreateCommand())
            {
                comm.CommandText =
                    "select id, trial, startingAgeInterval, endingAgeInterval from competitions where id=@id";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        int idV = dataR.GetInt32(0);
                        String trial = dataR.GetString(1);
                        int startingAgeInterval = dataR.GetInt32(2);
                        int endingAgeInterval = dataR.GetInt32(3);
                        Competition competition = new Competition(trial, startingAgeInterval, endingAgeInterval);
                        competition.Id = idV;
                        log.InfoFormat("Exiting FindOne with value {0}", competition);
                        return competition;
                    }
                }
            }

            return null;
        }

        private Participant findPart(long id)
        {
            log.InfoFormat("Entering FindOne with value {0}", id);
            IDbConnection con = DbUtils.getConnection(Props);
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select id, name, birthDate from participants where id=@id";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        int idV = dataR.GetInt32(0);
                        String name = dataR.GetString(1);
                        DateTime age = dataR.GetDateTime(2);
                        Participant participant = new Participant(name, age);
                        participant.Id = idV;
                        log.InfoFormat("Exiting FindOne with value {0}", participant);
                        return participant;
                    }
                }
            }

            throw new NotImplementedException();
        }

        public IEnumerable<Registration> FindAll()
        {
            log.InfoFormat("Entering FindAll");
            IDbConnection con = DbUtils.getConnection(Props);
            IList<Registration> registrations = new List<Registration>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select id, idParticipant, idCompetition from registrations";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        int participantId = dataR.GetInt32(1);
                        Participant participant = findPart(participantId);
                        int compId = dataR.GetInt32(2);
                        Competition competition = findComp(compId);
                        Registration registration = new Registration(participant, competition);
                        registration.Id = id;
                        log.InfoFormat("FindAll value {0}", registration);
                        registrations.Add(registration);
                    }
                }
            }

            return registrations;
        }

        public Registration findOne(long id)
        {
            throw new NotImplementedException();
        }

        public Registration save(Registration entity)
        {
            log.InfoFormat("Entering Save with value {0}", entity);
            IDbConnection con = DbUtils.getConnection(Props);
            using (var comm = con.CreateCommand())
            {
                comm.CommandText =
                    "insert into registrations (idParticipant, idCompetition) values (@idParticipant, @idCompetition)";
                IDbDataParameter paramIdPar = comm.CreateParameter();
                paramIdPar.ParameterName = "@idParticipant";
                paramIdPar.Value = entity.participant.Id;
                comm.Parameters.Add(paramIdPar);

                IDbDataParameter paramIdCom = comm.CreateParameter();
                paramIdCom.ParameterName = "@idCompetition";
                paramIdCom.Value = entity.competition.Id;
                comm.Parameters.Add(paramIdCom);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                {
                    log.InfoFormat("Exiting Save with value {0}", null);
                    return null;
                }
                else
                {
                    log.InfoFormat("Exiting Save with value {0}", entity);
                    return entity;
                }
            }
        }

        public Registration delete(long id)
        {
            log.InfoFormat("Entering Delete with value {0}", id);
            IDbConnection con = DbUtils.getConnection(Props);
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "delete from registrations where id=@id";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                {
                    log.InfoFormat("Exiting Delete with value {0}", null);
                    return null;
                }
                else
                {
                    log.InfoFormat("Exiting Delete with value {0}", id);
                }
            }

            throw new NotImplementedException();
        }

        public Registration update(Registration entity)
        {
            log.InfoFormat("Entering Update with value {0}", entity);
            IDbConnection con = DbUtils.getConnection(Props);
            using (var comm = con.CreateCommand())
            {
                comm.CommandText =
                    "update registrations set idParticipant=@idParticipant, idCompetition=@idCompetition where id=@id";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = entity.Id;
                comm.Parameters.Add(paramId);

                IDbDataParameter paramIdPar = comm.CreateParameter();
                paramIdPar.ParameterName = "@idParticipant";
                paramIdPar.Value = entity.participant.Id;
                comm.Parameters.Add(paramIdPar);

                IDbDataParameter paramIdCom = comm.CreateParameter();
                paramIdCom.ParameterName = "@idCompetition";
                paramIdCom.Value = entity.competition.Id;
                comm.Parameters.Add(paramIdCom);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                {
                    log.InfoFormat("Exiting Update with value {0}", null);
                    return null;
                }
                else
                {
                    log.InfoFormat("Exiting Update with value {0}", entity);
                    return entity;
                }
            }
        }

        public int getNrOfParticipants(Competition competition)
        {
            log.InfoFormat("Entering getNrOfParticipants with value {0}", competition);
            IDbConnection con = DbUtils.getConnection(Props);
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select count(*) from registrations where idCompetition=@idCompetition";
                IDbDataParameter paramIdCom = comm.CreateParameter();
                paramIdCom.ParameterName = "@idCompetition";
                paramIdCom.Value = competition.Id;
                comm.Parameters.Add(paramIdCom);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        int nr = dataR.GetInt32(0);
                        log.InfoFormat("Exiting getNrOfParticipants with value {0}", nr);
                        return nr;
                    }
                }
            }

            return 0;
        }

        public IEnumerable<Participant> getParticipantsForCompetition(long competitionId)
        {
            log.InfoFormat("Entering getParticipantsForCompetition with value {0}", competitionId);
            IDbConnection con = DbUtils.getConnection(Props);
            IList<Participant> participants = new List<Participant>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select idParticipant from registrations where idCompetition=@idCompetition";
                IDbDataParameter paramIdCom = comm.CreateParameter();
                paramIdCom.ParameterName = "@idCompetition";
                paramIdCom.Value = competitionId;
                comm.Parameters.Add(paramIdCom);

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        Participant participant = findPart(id);
                        log.InfoFormat("getParticipantsForCompetition value {0}", participant);
                        participants.Add(participant);
                    }
                }
            }

            return participants;
        }
    }
}