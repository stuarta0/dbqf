using dbqf.Configuration;
using dbqf.Sql.Configuration;
using dbqf.Sql;
using Standalone.Core.Data.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Core.Data
{
    public class ResultFactory
    {
        private int _commandTimeout;
        public ResultFactory()
        {
            _commandTimeout = 30;
        }
        public ResultFactory(int commandTimeout)
        {
            _commandTimeout = commandTimeout;
        }

        public ExposedSqlGenerator CreateSqlGenerator(IMatrixConfiguration configuration)
        {
            return new ExposedSqlGenerator(configuration);
        }

        public SqlListGenerator CreateSqlListGenerator(IMatrixConfiguration configuration)
        {
            return new SqlListGenerator(configuration);
        }

        public ISqlResults CreateSqlResults(Connection connection)
        {
            ISqlResults result = null;
            switch (connection.ConnectionType)
            {
                case "SqlClient":
                    result = new SqlResults(connection.ConnectionString);
                    break;
                case "SQLite":
                    result = new SQLiteResults(connection.ConnectionString);
                    break;
            }

            if (result != null)
            {
                result.CommandTimeout = _commandTimeout;
                return result;
            }

            throw new NotImplementedException(String.Concat("Could not create ISqlResults for connection ", connection.ConnectionType));
        }
    }
}
