using dbqf.Configuration;
using dbqf.Sql.Configuration;
using dbqf.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Core.Data
{
    public class DbServiceFactory
    {
        private IMatrixConfiguration _configuration;
        private int _commandTimeout;

        public DbServiceFactory(IMatrixConfiguration configuration)
            : this(configuration, 30) { }

        public DbServiceFactory(IMatrixConfiguration configuration, int commandTimeout)
        {
            _configuration = configuration;
            _commandTimeout = commandTimeout;
        }

        public IDbServiceAsync CreateAsync(ProjectConnection connection)
        {
            // TODO: implement command timeout
            if (connection is SqlProjectConnection)
                return new SqlService(_configuration, connection.ConnectionString);
            else if (connection is SQLiteProjectConnection)
                return new SQLiteService(_configuration, connection.ConnectionString);
            else if (connection is MsAccessProjectConnection)
                return new MsAccessService(_configuration, connection.ConnectionString);
               
            throw new NotImplementedException(String.Format("Could not create IDbServiceAsync for connection {0} ({1})", connection.DisplayName, connection.Identifier));
        }
    }
}
