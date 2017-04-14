using dbqf.Sql.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace dbqf.Hierarchy.Data
{
    public class SQLiteDataSource : SqlDataSource
    {
        public SQLiteDataSource(IMatrixConfiguration configuration, string connectionString)
            : base(configuration, connectionString)
        { }

        protected override IDbConnection CreateConnection()
        {
            return new SQLiteConnection(_connectionString);
        }

        protected override IDbDataAdapter CreateDataAdapter()
        {
            return new SQLiteDataAdapter();
        }
    }
}
