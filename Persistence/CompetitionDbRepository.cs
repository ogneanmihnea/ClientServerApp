using System;
using System.Collections.Generic;
using System.Data;
using Model;
using log4net;

namespace Persistence
{

    public class CompetitionDbRepository : ICompetitionRepository
    {
        private static readonly ILog log = LogManager.GetLogger("CompetitionDbRepository");
        IDictionary<string, string> props;

        public CompetitionDbRepository(IDictionary<string, string> props)
        {
            log.Info("Creating CompetitionDBRepository");
            this.props = props;
        }

        public IEnumerable<Competition> FindAll()
        {
            log.InfoFormat("Entering FindAll");
            IDbConnection con = DbUtils.getConnection(props);
            IList<Competition> competitions = new List<Competition>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select id, trial, startingAgeInterval, endingAgeInterval from competitions";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        String trial = dataR.GetString(1);
                        int startingAgeInterval = dataR.GetInt32(2);
                        int endingAgeInterval = dataR.GetInt32(3);
                        Competition competition = new Competition(trial, startingAgeInterval, endingAgeInterval);
                        competition.Id = id;
                        competitions.Add(competition);
                        log.InfoFormat("Competition found with value {0}", competition);
                    }
                }
            }

            return competitions;
        }

        public Competition findOne(long id)
        {
            log.InfoFormat("Entering FindOne with value {0}", id);
            IDbConnection con = DbUtils.getConnection(props);
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "id, trial, startingAgeInterval, endingAgeInterval from competitions where id=@id";
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

        public Competition save(Competition entity)
        {
            log.InfoFormat("Entering Save with value {0}", entity);
            IDbConnection con = DbUtils.getConnection(props);
            using (var comm = con.CreateCommand())
            {
                comm.CommandText =
                    "insert into competitions (trial, startingAgeInterval, endingAgeInterval) values (@trial, @startingAgeInterval, @endingAgeInterval)";
                IDbDataParameter paramTrial = comm.CreateParameter();
                paramTrial.ParameterName = "@trial";
                paramTrial.Value = entity.trial;
                comm.Parameters.Add(paramTrial);

                IDbDataParameter paramSAge = comm.CreateParameter();
                paramSAge.ParameterName = "@startingAgeInterval";
                paramSAge.Value = entity.startingAgeInterval;
                comm.Parameters.Add(paramSAge);


                IDbDataParameter paramEAge = comm.CreateParameter();
                paramEAge.ParameterName = "@endingAgeInterval";
                paramEAge.Value = entity.endingAgeInterval;
                comm.Parameters.Add(paramEAge);

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

        public Competition delete(long id)
        {
            log.InfoFormat("Entering Delete with value {0}", id);
            IDbConnection con = DbUtils.getConnection(props);
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "delete from competitions where id=@id";
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
                    Competition competition = new Competition();
                    competition.Id = id;
                    log.InfoFormat("Exiting Delete with value {0}", competition);
                    return competition;
                }
            }
        }

        public Competition update(Competition entity)
        {

            log.InfoFormat("Entering Update with value {0}", entity);
            IDbConnection con = DbUtils.getConnection(props);
            using (var comm = con.CreateCommand())
            {
                comm.CommandText =
                    "update competitions set trial=@trial, startingAgeInterval=@startingAgeInterval, endingAgeInterval=@endingAgeInterval where id=@id";

                IDbDataParameter paramTrial = comm.CreateParameter();
                paramTrial.ParameterName = "@trial";
                paramTrial.Value = entity.trial;
                comm.Parameters.Add(paramTrial);

                IDbDataParameter paramSAge = comm.CreateParameter();
                paramSAge.ParameterName = "@startingAgeInterval";
                paramSAge.Value = entity.startingAgeInterval;
                comm.Parameters.Add(paramSAge);

                IDbDataParameter paramEAge = comm.CreateParameter();
                paramEAge.ParameterName = "@endingAgeInterval";
                paramEAge.Value = entity.endingAgeInterval;
                comm.Parameters.Add(paramEAge);

                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = entity.Id;
                comm.Parameters.Add(paramId);

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
    }
}