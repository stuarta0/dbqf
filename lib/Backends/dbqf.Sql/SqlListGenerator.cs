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
    public class SqlListGenerator
    {
        protected IMatrixConfiguration _configuration;
        protected IFieldPath _path;
        protected string _idColumn;
        protected string _valueColumn;
        protected string _sortByColumn;

        public SqlListGenerator(IMatrixConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlListGenerator Path(IFieldPath path)
        {
            _path = path;
            return this;
        }

        public SqlListGenerator IdColumn(string name)
        {
            _idColumn = name;
            return this;
        }

        public SqlListGenerator ValueColumn(string name)
        {
            _valueColumn = name;
            return this;
        }

        public SqlListGenerator SortBy(string name)
        {
            _sortByColumn = name;
            return this;
        }

        public void Validate()
        {
            if (_path == null || _path.Count == 0)
                throw new Exception("No path given to resolve list data.");
            if (_path.Last.List == null || String.IsNullOrEmpty(_path.Last.List.Source))
                throw new ArgumentException(String.Concat("No list source provided for ", _path.Description, "."));

            foreach (var f in _path)
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
            if (!String.IsNullOrEmpty(_idColumn) && !String.IsNullOrEmpty(_valueColumn))
            {
                // since the list source will be (by definition) the ID's of it's corresponding subject, we start with the next field back (count - 2)
                var sb = new StringBuilder();
                sb.AppendFormat("SELECT DISTINCT q{0}.[{1}] FROM ({2}) AS q{0} ", _path.Count - 1, _valueColumn, _path.Last.List.Source);
                for (int i = _path.Count - 2; i >= 0; i--)
                {
                    sb.AppendFormat("INNER JOIN ({2}) AS q{0} ON q{0}.[{3}] = q{1}.[{4}] ",
                        i, i + 1,
                        ((ISqlSubject)_path[i].Subject).Sql,
                        _path[i].SourceName,
                        (i + 1 == _path.Count - 1 ? _idColumn : _path[i + 1].Subject.IdField.SourceName));
                }

                sb.AppendFormat("ORDER BY q{0}.[{1}] ", _path.Count - 1, _sortByColumn ?? _valueColumn);
                cmd.CommandText = sb.ToString();
            }
            else
            {
                cmd.CommandText = _path.Last.List.Source;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
