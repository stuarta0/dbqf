﻿using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Sql;
using dbqf.Sql.Configuration;
using dbqf.Sql.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

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

        protected virtual ISqlListGenerator GetListGenerator()
        {
            return new SqlListGenerator(_config);
        }

        public DataTable GetResults(ISearchDetails details)
        {
            if (!(details.Target is ISqlSubject))
                throw new ArgumentException("Target subject must be of type ISqlSubject.");
            if (details.Where != null && !(details.Where is ISqlParameter))
                throw new ArgumentException("Where parameter must be of type ISqlParameter.");

            var generator = GetGenerator();
            generator.Target = (ISqlSubject)details.Target;
            generator.Columns = details.Columns;
            generator.Where = (ISqlParameter)details.Where;

            using (var conn = CreateConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    //cmd.CommandTimeout = CommandTimeout;
                    generator.UpdateCommand(cmd);
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

        public List<object> GetList(IFieldPath path)
        {
            var generator = GetListGenerator();
            generator.Path = path;

            if (Regex.IsMatch(path.Last.List.Source, @"^select.*[`'\[\s]id", RegexOptions.IgnoreCase))
            {
                generator.IdColumn = "ID";
                generator.ValueColumn = "Value";
            }

            HashSet<object> set = new HashSet<object>();
            using (var conn = CreateConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    //cmd.CommandTimeout = CommandTimeout;
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

        public Action GetResults(ISearchDetails details, IDbServiceAsyncCallback<DataTable> callback) //ResultCallback callback)
        {
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
                    callback.Exception = e2.Error;
                }
                else
                {
                    callback.Results = (DataTable)e2.Result;
                    //callback(details, (DataTable)e2.Result);
                }
                callback.Callback(callback);
            };
            worker.RunWorkerAsync();
            return new Action(worker.CancelAsync);
        }

        public Action GetList(IFieldPath path, IDbServiceAsyncCallback<List<object>> callback) //ListCallback callback)
        {
            var worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;

            worker.DoWork += (s1, e1) =>
            {
                e1.Result = GetList(path);
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
                    callback.Exception = e2.Error;
                }
                else
                {
                    callback.Results = e2.Result as List<object>;
                    //callback(path, e2.Result as List<object>);
                }
                callback.Callback(callback);
            };
            worker.RunWorkerAsync();
            return new Action(worker.CancelAsync);
        }
    }
}
