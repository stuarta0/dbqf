using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Sql.Configuration;
using dbqf.Sql.Criterion;
using dbqf.Sql;
using System.Data.SqlClient;

namespace dbqf.Hierarchy.Data
{
    public class SqlDataSource : IDataSource
    {
        public SqlDataSource(IMatrixConfiguration config, string connectionString)
        {
            _config = config;
            _connectionString = connectionString;
        }
        readonly IMatrixConfiguration _config;
        readonly string _connectionString;

        public DataTable GetData(ISubject target, IList<IFieldPath> fields, IParameter where)
        {
            if (!(target is ISqlSubject))
                throw new ArgumentException("Target subject must be of type ISqlSubject.");
            if (where != null && !(where is ISqlParameter))
                throw new ArgumentException("Where parameter must be of type ISqlParameter.");

            var generator = new SqlGenerator(_config); // GetGenerator();
            generator.Target = (ISqlSubject)target;
            generator.Columns = fields;
            generator.Where = (ISqlParameter)where;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    //cmd.CommandTimeout = CommandTimeout;
                    generator.UpdateCommand(cmd);

                    var adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    var ds = new DataSet();
                    adapter.Fill(ds);
                    var result = ds.Tables[0];
                    if (adapter is IDisposable)
                        ((IDisposable)adapter).Dispose();

                    for (int i = 0; i < result.Columns.Count; i++)
                        result.Columns[i].ExtendedProperties.Add("FieldPath", fields[i]);

                    return result;
                }
            }
        }
    }
}
