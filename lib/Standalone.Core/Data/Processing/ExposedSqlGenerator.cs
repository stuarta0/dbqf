using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Configuration;
using dbqf.Processing;

namespace Standalone.Core.Data.Processing
{
    public class ExposedSqlGenerator : SqlGenerator
    {
        public ExposedSqlGenerator(IConfiguration configuration)
            : base(configuration)
        {
        }

        public string GenerateSql()
        {
            // this concrete implementation is throw-away
            var cmd = new System.Data.SqlClient.SqlCommand();
            UpdateCommand(cmd);

            var sb = new StringBuilder(cmd.CommandText);
            sb.AppendLine(Environment.NewLine);
            foreach (System.Data.SqlClient.SqlParameter p in cmd.Parameters)
                sb.AppendLine(String.Format("{0} = {1}", p.ParameterName, p.Value));
            return sb.ToString();
        }
    }
}
