using dbqf.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Processing;
using System.Data.SQLite;
using System.Data.Common;

namespace Standalone.Data
{
    public class SQLiteResults : DbResults
    {
        private string _connectionString;
        public SQLiteResults(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override DbConnection CreateConnection()
        {
            return new SQLiteConnection(_connectionString);
        }

        protected override System.Data.Common.DbDataAdapter CreateDataAdapter()
        {
            return new SQLiteDataAdapter();
        }
    }
}
