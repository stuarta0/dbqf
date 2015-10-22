using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Sql;
using dbqf.Sql.Configuration;
using dbqf.Sql.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Standalone.Core.Data
{
    public class SqlService : IDbService, IDbServiceAsync
    {
        protected IMatrixConfiguration _config;
        protected string _connectionString;
        public SqlService(IMatrixConfiguration configuration, string connectionString)
        {
            _config = configuration;
            _connectionString = connectionString;
        }

        /// <summary>
        /// Gets SQL for displaying to a user.  Doesn't need to be syntactically correct.
        /// </summary>
        protected virtual string GetSql(IDbCommand cmd)
        {
            var sb = new StringBuilder(cmd.CommandText);
            sb.AppendLine(Environment.NewLine);
            foreach (System.Data.IDataParameter p in cmd.Parameters)
                sb.AppendLine(String.Format("{0} = {1}", p.ParameterName, p.Value));
            return sb.ToString();
        }

        protected virtual IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        protected virtual IDbDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }

        protected virtual ISqlGenerator GetGenerator()
        {
            return new SqlGenerator(_config);
        }

        public DataTable GetResults(ISearchDetails details)
        {
            if (!(details.Target is ISqlSubject))
                throw new ArgumentException("Target subject must be of type ISqlSubject.");
            if (!(details.Where is ISqlParameter))
                throw new ArgumentException("Where parameter must be of type ISqlParameter.");

            var gen = GetGenerator()
                .Target((ISqlSubject)details.Target)
                .Column(details.Columns)
                .Where((ISqlParameter)details.Where);

            using (var conn = CreateConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    //cmd.CommandTimeout = CommandTimeout;
                    gen.UpdateCommand(cmd);
                    details.Sql = GetSql(cmd);

                    var adapter = CreateDataAdapter();
                    adapter.SelectCommand = cmd;
                    var ds = new DataSet();
                    adapter.Fill(ds);
                    var result = ds.Tables[0];
                    if (adapter is IDisposable)
                        ((IDisposable)adapter).Dispose();

                    for (int i = 0; i < result.Columns.Count; i++)
                        result.Columns[i].ExtendedProperties.Add("FieldPath", details.Columns[i]);

                    return result;
                }
            }
        }

        public List<string> GetList(IFieldPath path)
        {
            throw new NotImplementedException();
        }

        public void GetResults(ISearchDetails details, ResultCallback callback)
        {
            if (!(details.Target is ISqlSubject))
                throw new ArgumentException("Target subject must be of type ISqlSubject.");
            if (!(details.Where is ISqlParameter))
                throw new ArgumentException("Where parameter must be of type ISqlParameter.");

            var worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;

            worker.DoWork += (s1, e1) =>
            {
                e1.Result = GetResults(details);
                e1.Cancel = worker.CancellationPending;
            };
            worker.RunWorkerCompleted += (s2, e2) =>
            {
                worker.Dispose();

                // cancellation assumes the SearchWorker property has been set null
                if (e2.Cancelled)
                    return;

                if (e2.Error != null)
                {
                    //MessageBox.Show("There was an error when trying to perform the search.\n\n" + e2.Error.Message, "Search", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                    callback(details, (DataTable)e2.Result);
            };
            worker.RunWorkerAsync();
        }

        public void GetList(IFieldPath path, ListCallback callback)
        {
            throw new NotImplementedException();
        }
    }
}
