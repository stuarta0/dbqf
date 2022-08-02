using dbqf.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Sql;

namespace Standalone.Core.Data
{
    public interface ISqlResults
    {
        int CommandTimeout { get; set; }
        DataTable GetResults(SqlGenerator cmd);
        IList<object> GetList(SqlListGenerator cmd);
    }
}
