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

namespace Standalone.Data
{
    public abstract class DbResults : ISqlResults
    {
        protected abstract DbConnection CreateConnection();
        protected abstract DbDataAdapter CreateDataAdapter();

        public virtual DataTable GetResults(SqlGenerator generator)
        {
            DataTable result = new DataTable();
            using (var conn = CreateConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    generator.UpdateCommand(cmd);
                    using (var adapter = CreateDataAdapter())
                    {
                        adapter.SelectCommand = cmd;
                        adapter.Fill(result);
                    }
                }
            }
            return result;
        }

        public virtual IList<object> GetList(SqlListGenerator generator)
        {
            HashSet<object> set = new HashSet<object>();
            using (var conn = CreateConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    generator.UpdateCommand(cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        int idx = -1;
                        try { idx = reader.GetOrdinal("ID"); }
                        catch { }

                        while (reader.Read())
                        {
                            // assumes either 1 or 2 columns. If 2, one will be ID column
                            var item = reader.GetValue(idx != -1 ? 1 - idx : 0);
                            if (!set.Contains(item))
                                set.Add(item);
                        }
                    }
                }
            }
            return new List<object>(set);
        }
    }
}
