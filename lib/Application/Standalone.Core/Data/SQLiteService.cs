using dbqf.Sql.Configuration;
using System.Data;
using System.Data.SQLite;

namespace Standalone.Core.Data
{
    public class SQLiteService : SqlService
    {
        public SQLiteService(IMatrixConfiguration configuration, string connectionString)
            : base(configuration, connectionString) { }

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
