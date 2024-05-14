using System;
using System.Collections.Generic;
using System.Data;
using log4net;
using Model;

namespace Persistence
{

	public class UserDbRepository : IUserRepository
	{

		private static readonly ILog log = LogManager.GetLogger("UserDBRepository");

		IDictionary<String, string> props;

		public UserDbRepository(IDictionary<String, string> props)
		{
			log.Info("Creating UserDBRepository");
			this.props = props;
		}



		public IEnumerable<User> FindAll()
		{
			log.InfoFormat("Entering FindAll");
			IDbConnection con = DbUtils.getConnection(props);
			IList<User> users = new List<User>();
			using (var comm = con.CreateCommand())
			{
				comm.CommandText = "select id, username, password from users";

				using (var dataR = comm.ExecuteReader())
				{
					while (dataR.Read())
					{
						int id = dataR.GetInt32(0);
						String username = dataR.GetString(1);
						String password = dataR.GetString(2);
						User user = new User(username, password);
						user.Id = id;
						users.Add(user);
						log.InfoFormat("User found with value {0}", user);
					}
				}
			}

			return users;
		}

		public User findOne(long id)
		{
			throw new NotImplementedException();
		}


		public User save(User user)
		{
			log.InfoFormat("Entering Save with value {0}", user);
			IDbConnection con = DbUtils.getConnection(props);
			using (var comm = con.CreateCommand())
			{
				comm.CommandText = "insert into users (username, password) values (@username, @password)";
				IDbDataParameter paramUsername = comm.CreateParameter();
				paramUsername.ParameterName = "@username";
				paramUsername.Value = user.username;
				comm.Parameters.Add(paramUsername);

				IDbDataParameter paramPassword = comm.CreateParameter();
				paramPassword.ParameterName = "@password";
				paramPassword.Value = user.password;
				comm.Parameters.Add(paramPassword);

				var result = comm.ExecuteNonQuery();
				if (result == 0)
				{
					log.InfoFormat("Exiting Save with value {0}", null);
					return null;
				}
				else
				{
					log.InfoFormat("Exiting Save with value {0}", user);
					return user;
				}
			}
		}

		public User delete(long id)
		{
			log.InfoFormat("Entering Delete with value {0}", id);
			IDbConnection con = DbUtils.getConnection(props);
			using (var comm = con.CreateCommand())
			{
				comm.CommandText = "delete from users where id=@id";
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
					return new User();
				}
			}
		}

		public User update(User entity)
		{
			log.InfoFormat("Entering Update with value {0}", entity);
			IDbConnection con = DbUtils.getConnection(props);
			using (var comm = con.CreateCommand())
			{
				comm.CommandText = "update users set username=@username, password=@password where id=@id";
				IDbDataParameter paramUsername = comm.CreateParameter();
				paramUsername.ParameterName = "@username";
				paramUsername.Value = entity.username;
				comm.Parameters.Add(paramUsername);

				IDbDataParameter paramPassword = comm.CreateParameter();
				paramPassword.ParameterName = "@password";
				paramPassword.Value = entity.password;
				comm.Parameters.Add(paramPassword);

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

		public User FindByUsername(string username)
		{
			log.InfoFormat("Entering FindByUsername with value {0}", username);
			IDbConnection con = DbUtils.getConnection(props);
			using (var comm = con.CreateCommand())
			{
				comm.CommandText = "select id, username, password from users where username=@username";
				IDbDataParameter paramUsername = comm.CreateParameter();
				paramUsername.ParameterName = "@username";
				paramUsername.Value = username;
				comm.Parameters.Add(paramUsername);
				using (var dataR = comm.ExecuteReader())
				{
					if (dataR.Read())
					{
						int id = dataR.GetInt32(0);
						String username1 = dataR.GetString(1);
						String password = dataR.GetString(2);
						User user = new User(username1, password);
						user.Id = id;
						log.InfoFormat("Exiting FindByUsername with value {0}", user);
						return user;
					}
				}
			}

			log.InfoFormat("Exiting FindByUsername with value {0}", null);
			return null;
		}
	}
}