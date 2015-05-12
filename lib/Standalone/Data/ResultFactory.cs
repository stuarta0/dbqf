using dbqf.Configuration;
using dbqf.Processing;
using Standalone.Data.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Data
{
    public class ResultFactory
    {
        public ExposedSqlGenerator CreateSqlGenerator(Connection connection, IConfiguration configuration)
        {
            return new ExposedSqlGenerator(configuration);
        }

        public SqlListGenerator CreateSqlListGenerator(Connection connection, IConfiguration configuration)
        {
            return new SqlListGenerator(configuration);
        }

        public ISqlResults CreateSqlResults(Connection connection)
        {
            switch (connection.ConnectionType)
            {
                case "SqlClient":
                    return new SqlResults(connection.ConnectionString);
                    break;
                case "SQLite":
                    return new SQLiteResults(connection.ConnectionString);
                    break;
            }

            throw new NotImplementedException(String.Concat("Could not create ISqlResults for connection ", connection.ConnectionType));
        }
    }
}
