using dbqf.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Sql.Processing;

namespace Standalone.Core.Data
{
    public interface ISqlResults
    {
        int CommandTimeout { get; set; }
        DataTable GetResults(SqlGenerator cmd);
        IList<object> GetList(SqlListGenerator cmd);
    }
}
