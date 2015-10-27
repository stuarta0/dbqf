using dbqf.Sql;
using dbqf.Sql.Configuration;
using System;
using System.Data;
using System.Data.OleDb;

namespace Standalone.Core.Data
{
    public class MsAccessService : SqlService
    {
        public MsAccessService(IMatrixConfiguration configuration, string connectionString)
            : base(configuration, connectionString) { }

        protected override IDbConnection CreateConnection()
        {
            return new OleDbConnection(_connectionString);
        }

        protected override IDbDataAdapter CreateDataAdapter()
        {
            return new OleDbDataAdapter();
        }

        protected override ISqlGenerator GetGenerator()
        {
            return new dbqf.MsAccess.SqlGenerator(_config);
        }

        protected override ISqlListGenerator GetListGenerator()
        {
            return new dbqf.MsAccess.SqlListGenerator(_config);
        }
    }
}
