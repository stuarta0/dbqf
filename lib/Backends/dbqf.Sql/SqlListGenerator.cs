using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Sql.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace dbqf.Sql
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlListGenerator : ISqlListGenerator
    {
        protected IMatrixConfiguration _configuration;

        public SqlListGenerator(IMatrixConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IFieldPath Path { get; set; }
        public string IdColumn { get; set; }
        public string ValueColumn { get; set; }
        public string SortBy { get; set; }

        public SqlListGenerator WithPath(IFieldPath path)
        {
            Path = path;
            return this;
        }

        public SqlListGenerator IdColumnName(string name)
        {
            IdColumn = name;
            return this;
        }

        public SqlListGenerator ValueColumnName(string name)
        {
            ValueColumn = name;
            return this;
        }

        public SqlListGenerator SortByName(string name)
        {
            SortBy = name;
            return this;
        }

        public void Validate()
        {
            if (Path == null || Path.Count == 0)
                throw new Exception("No path given to resolve list data.");
            if (Path.Last.List == null || String.IsNullOrEmpty(Path.Last.List.Source))
                throw new ArgumentException(String.Concat("No list source provided for ", Path.Description, "."));

            foreach (var f in Path)
                if (!(f.Subject is ISqlSubject))
                    throw new ArgumentException("All fields must relate to ISqlSubjects.");
        }

        /// <summary>
        /// Generate a command to use for determining list data for a given field path.
        /// </summary>
        /// <param name="dbCommandType"></param>
        /// <returns></returns>
        public virtual void UpdateCommand(IDbCommand cmd)
        {
            Validate();

            // create join statement through join from index 0, through each field with an inner join
            // to the last field.  If the last field List source does not contain an ID field, this 
            // will simply return the query defined by the field.

            cmd.Parameters.Clear();
            if (!String.IsNullOrEmpty(IdColumn) && !String.IsNullOrEmpty(ValueColumn))
            {
                // since the list source will be (by definition) the ID's of it's corresponding subject, we start with the next field back (count - 2)
                var sb = new StringBuilder();
                sb.AppendFormat("SELECT DISTINCT q{0}.[{1}] FROM ({2}) AS q{0} ", Path.Count - 1, ValueColumn, Path.Last.List.Source);
                for (int i = Path.Count - 2; i >= 0; i--)
                {
                    sb.AppendFormat("INNER JOIN ({2}) AS q{0} ON q{0}.[{3}] = q{1}.[{4}] ",
                        i, i + 1,
                        ((ISqlSubject)Path[i].Subject).Sql,
                        Path[i].SourceName,
                        (i + 1 == Path.Count - 1 ? IdColumn : Path[i + 1].Subject.IdField.SourceName));
                }

                sb.AppendFormat("ORDER BY q{0}.[{1}] ", Path.Count - 1, SortBy ?? ValueColumn);
                cmd.CommandText = sb.ToString();
            }
            else
            {
                cmd.CommandText = Path.Last.List.Source;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
