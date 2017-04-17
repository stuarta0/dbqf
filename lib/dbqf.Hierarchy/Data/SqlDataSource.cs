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
        protected readonly IMatrixConfiguration _config;
        protected readonly string _connectionString;

        public DataTable GetData(ISubject target, IList<IFieldPath> fields, IParameter where)
        {
            return GetData(target, fields, where, null);
        }

        public DataTable GetData(ISubject target, IList<IFieldPath> fields, IParameter where, IList<OrderedField> orderBy)
        {
            if (!(target is ISqlSubject))
                throw new ArgumentException("Target subject must be of type ISqlSubject.");
            if (where != null && !(where is ISqlParameter))
                throw new ArgumentException("Where parameter must be of type ISqlParameter.");

            var generator = new SqlGenerator(_config); // GetGenerator();
            generator.Target = (ISqlSubject)target;
            generator.Columns = fields;
            generator.Where = (ISqlParameter)where;
            if (orderBy != null)
                foreach (var f in orderBy)
                    generator.OrderBy(f.FieldPath, f.SortDirection);

            using (var conn = CreateConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    //cmd.CommandTimeout = CommandTimeout;
                    generator.UpdateCommand(cmd);

                    var adapter = CreateDataAdapter();
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

        protected virtual IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        protected virtual IDbDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }
    }
}
