using System;
using System.Collections.Generic;
using System.Data;
using log4net;
using Model;

namespace Persistence
{

    public class ParticipantDbRepository : IParticipantRepository
    {
        private static readonly ILog log = LogManager.GetLogger("ParticipantDbRepository");

        IDictionary<String, string> props;

        public ParticipantDbRepository(IDictionary<String, string> props)
        {
            log.Info("Creating ParticipantDBRepository");
            this.props = props;
        }

        public IEnumerable<Participant> FindAll()
        {
            log.InfoFormat("Entering FindAll");
            IDbConnection con = DbUtils.getConnection(props);
            IList<Participant> participants = new List<Participant>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select id, name, birthDate from participants";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        String name = dataR.GetString(1);
                        DateTime age = dataR.GetDateTime(2);
                        Participant participant = new Participant(name, age);
                        participant.Id = id;
                        participants.Add(participant);
                    }
                }
            }

            return participants;
        }


        public Participant findOne(long id)
        {
            log.InfoFormat("Entering FindOne with value {0}", id);
            IDbConnection con = DbUtils.getConnection(props);
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

        public Participant save(Participant entity)
        {
            log.InfoFormat("Entering Save with value {0}", entity);
            IDbConnection con = DbUtils.getConnection(props);
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "insert into participants (name, birthDate) values (@name, @birthDate)";
                IDbDataParameter paramName = comm.CreateParameter();
                paramName.ParameterName = "@name";
                paramName.Value = entity.name;
                comm.Parameters.Add(paramName);
                IDbDataParameter paramAge = comm.CreateParameter();
                paramAge.ParameterName = "@birthDate";
                paramAge.Value = entity.birthDate;
                comm.Parameters.Add(paramAge);
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

        public Participant delete(long id)
        {
            log.InfoFormat("Entering Delete with value {0}", id);
            IDbConnection con = DbUtils.getConnection(props);
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "delete from participants where id=@id";
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
                    return new Participant("", DateTime.Now);
                }
            }
        }

        public Participant update(Participant entity)
        {
            log.InfoFormat("Entering Update with value {0}", entity);
            IDbConnection con = DbUtils.getConnection(props);
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "update participants set name=@name, birthDate=@birthDate where id=@id";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = entity.Id;
                comm.Parameters.Add(paramId);
                IDbDataParameter paramName = comm.CreateParameter();
                paramName.ParameterName = "@name";
                paramName.Value = entity.name;
                comm.Parameters.Add(paramName);
                IDbDataParameter paramAge = comm.CreateParameter();
                paramAge.ParameterName = "@birthDate";
                paramAge.Value = entity.birthDate;
                comm.Parameters.Add(paramAge);
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

        public Participant findByName(String name)
        {
            log.InfoFormat("Entering FindByName with value {0}", name);
            IDbConnection con = DbUtils.getConnection(props);
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select id, name, birthDate from participants where name=@name";
                IDbDataParameter paramName = comm.CreateParameter();
                paramName.ParameterName = "@name";
                paramName.Value = name;
                comm.Parameters.Add(paramName);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        String nameV = dataR.GetString(1);
                        DateTime age = dataR.GetDateTime(2);
                        Participant participant = new Participant(nameV, age);
                        participant.Id = id;
                        log.InfoFormat("Exiting FindByName with value {0}", participant);
                        return participant;
                    }
                }
            }

            throw new NotImplementedException();
        }
    }
}