using dbqf.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Processing;
using System.Data.Common;

namespace Standalone.Core.Data
{
    public class SqlResults : DbResults
    {
        private string _connectionString;
        public SqlResults(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override DbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        protected override DbDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }
    }
}
