using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Processing;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Standalone.Core.Data.Processing;
using System.Text.RegularExpressions;

namespace Sandbox
{
    class ConfigurationValidation
    {
        private IConfiguration _configuration;
        private DbConnection _conn;
        public ConfigurationValidation(IConfiguration config, DbConnection conn)
        {
            _configuration = config;
            _conn = conn;
        }

        /// <summary>
        /// Execute all SQL statements and ensure the given configuration is valid.
        /// </summary>
        /// <param name="config"></param>
        public virtual void Validate(bool onlyErrors = false)
        {
            _conn.Open();
            var broken = new List<ISubject>();

            // ensure all the source data from the subjects can be executed as SQL
            foreach (var subject in _configuration)
            {
                // check subject and all non-related fields first
                var fields = subject.FindAll<IField>(f => !(f is IRelationField))
                    .ToList<IField>()
                    .ConvertAll<IFieldPath>(f => FieldPath.FromDefault(f));

                var generator = new ExposedSqlGenerator(_configuration);
                generator
                    .Target(subject)
                    .Column(fields);
                
                try
                {
                    int rows = Execute(generator);
                    if (!onlyErrors)
                        Console.WriteLine("Subject '" + subject.DisplayName + "' succeeded (" + rows + " rows affected)");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Subject '" + subject.DisplayName + "' failed when executing with all non-RelationFields: " + ex.Message + "\n\t" + generator.GenerateSql());
                    broken.Add(subject);
                    continue;
                }


                // now check all RelationFields individually for validity
                foreach (var rf in subject.FindAll<IField>(f => f is IRelationField))
                {
                    var path = FieldPath.FromDefault(rf);

                    var bf = path.Find(f2 => broken.Contains(f2.Subject));
                    if (bf != null)
                    {
                        Console.WriteLine("Cannot check relation field for " + path.Description + " as " + bf.Subject + " SQL is invalid.");
                        continue;
                    }

                    var generator2 = new ExposedSqlGenerator(_configuration);
                    generator2
                        .Target(subject)
                        .Column(path);
                
                    try
                    {
                        int rows = Execute(generator2);
                        if (!onlyErrors)
                            Console.WriteLine("RelationField '" + rf.DisplayName + "' succeeded.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("RelationField '" + rf.DisplayName + "' failed. " + ex.Message + "\n\t" + generator2.GenerateSql());
                    }
                }

                // for all fields, ensure list data can be retrieved
                foreach (var f in subject)
                {
                    var path = FieldPath.FromDefault(f);
                    if (path.Last.List != null && !String.IsNullOrEmpty(path.Last.List.Source))
                    {
                        var bf = path.Find(f2 => broken.Contains(f2.Subject));
                        if (bf != null)
                        {
                            Console.WriteLine("Cannot check list data for " + path.Description + " as " + bf.Subject + " SQL is invalid.");
                            continue;
                        }

                        var gen = new SqlListGenerator(_configuration).Path(path);
                        if (Regex.IsMatch(path.Last.List.Source, @"^select.*[`'\[\s]id", RegexOptions.IgnoreCase))
                            gen.IdColumn("ID")
                                .ValueColumn("Value");

                        try
                        {
                            int rows = Execute(gen);
                            if (!onlyErrors)
                                Console.WriteLine("List data for " + path.Description + " succeeded (" + rows + " rows affected).");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("List data for " + path.Description + " failed. " + ex.Message + "\n\t");
                        }
                    }
                }
            }


            // ensure all path SQL statements execute successfully
            foreach (var from in _configuration)
            {
                foreach (var to in _configuration)
                {
                    if (broken.Contains(from))
                    {
                        Console.WriteLine("Cannot check matrix query [" + from.DisplayName + " to " + to.DisplayName + "] because " + from.DisplayName + " SQL is invalid.");
                        continue;
                    }

                    if (broken.Contains(to))
                    {
                        Console.WriteLine("Cannot check matrix query [" + from.DisplayName + " to " + to.DisplayName + "] because " + to.DisplayName + " SQL is invalid.");
                        continue;
                    }

                    var matrix = _configuration[from, to];

                    if (String.IsNullOrWhiteSpace(matrix.Query))
                    {
                        if (!onlyErrors)
                            Console.WriteLine("No relationship between " + from.DisplayName + " to " + to.DisplayName + ".");
                        continue;
                    }

                    // try the vanilla SQL first
                    try
                    {
                        int rows = Execute(matrix.Query);
                        if (!onlyErrors)
                            Console.WriteLine("Matrix query for [" + from.DisplayName + " to " + to.DisplayName + "] succeeded (" + rows + " rows affected).");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Matrix query for [" + from.DisplayName + " to " + to.DisplayName + "] failed. " + ex.Message + "\n\t" + matrix.Query);
                        continue;
                    }


                    // now try a full query traversing the two subjects
                    // TODO: is this correct?  are we asking for Target=from?  somethings not right here...
                    var generator = new ExposedSqlGenerator(_configuration);
                    generator
                        .Target(from)
                        .Column(FieldPath.FromDefault(to.IdField));

                    try
                    {
                        int rows = Execute(generator);
                        if (!onlyErrors)
                            Console.WriteLine("Generated SQL for " + from.DisplayName + " to " + to.DisplayName + " succeeded (" + rows + " rows affected).");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Generated SQL for " + from.DisplayName + " to " + to.DisplayName + " failed. " + ex.Message + "\n\t" + generator.GenerateSql());
                    }
                }
            }
        }

        private int Execute(SqlGenerator gen)
        {
            using (var cmd = _conn.CreateCommand())
            {
                gen.UpdateCommand(cmd);
                return cmd.ExecuteNonQuery();
            }
        }

        private int Execute(string sql)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;
                return cmd.ExecuteNonQuery();
            }
        }

        private int Execute(SqlListGenerator generator)
        {
            using (var cmd = _conn.CreateCommand())
            {
                generator.UpdateCommand(cmd);
                return cmd.ExecuteNonQuery();
            }

        }
    }
}
