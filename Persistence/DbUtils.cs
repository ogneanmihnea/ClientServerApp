using System.Collections.Generic;
using System.Data;

namespace Persistence
{

    public static class DbUtils
    {


        private static IDbConnection instance = null;


        public static IDbConnection getConnection(IDictionary<string, string> props)
        {
            if (instance == null || instance.State == System.Data.ConnectionState.Closed)
            {
                instance = getNewConnection(props);
                instance.Open();
            }

            return instance;
        }

        private static IDbConnection getNewConnection(IDictionary<string, string> props)
        {
            return utils.DbUtils.ConnectionFactory.getInstance().createConnection(props);
        }
    }
}