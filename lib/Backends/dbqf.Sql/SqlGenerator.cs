using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Processing;
using dbqf.Sql.Configuration;
using dbqf.Sql.Criterion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace dbqf.Sql
{
    public class SqlGenerator : ISqlGenerator
    {
        protected IMatrixConfiguration _configuration;
        protected IList<IFieldPath> _columns;
        protected ISqlSubject _target;
        protected ISqlParameter _where;
        protected List<IFieldPath> _groupBy;
        protected List<OrderedField> _orderBy;

        public SqlGenerator(IMatrixConfiguration configuration)
        {
            _configuration = configuration;
            _columns = new List<IFieldPath>();
            _groupBy = new List<IFieldPath>();
            _orderBy = new List<OrderedField>();
            AliasColumns = true;
        }

        protected class OrderedField
        {
            public IFieldPath Path;
            public string Direction;

            public OrderedField(IFieldPath path, string direction)
            {
                Path = path;
                Direction = direction;
            }
        }

        public bool AliasColumns { get; set; }

        /// <summary>
        /// Adds a number of columns to retrieve.
        /// </summary>
        public IList<IFieldPath> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        /// <summary>
        /// Sets the core target that will be queried.
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public ISqlSubject Target
        {
            get { return _target; }
            set { _target = value; }
        }

        /// <summary>
        /// Sets the parameters to limit results.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public ISqlParameter Where
        {
            get { return _where; }
            set { _where = value; }
        }

        public virtual SqlGenerator Alias(bool alias)
        {
            AliasColumns = alias;
            return this;
        }

        /// <summary>
        /// Uses a column in the select list.  Any additional joins that are required to use these columns will be included.
        /// Can be called multiple times.
        /// </summary>
        public virtual SqlGenerator Column(IFieldPath path)
        {
            _columns.Add(path);
            return this;
        }

        public virtual SqlGenerator Column(IEnumerable<IFieldPath> path)
        {
            foreach (var p in path)
                Column(p);
            return this;
        }

        /// <summary>
        /// The target subject to search for results.  This is the first query defined after the FROM clause.
        /// Setting this multiple times will simply use the last call.
        /// </summary>
        public virtual SqlGenerator ForTarget(ISqlSubject subject)
        {
            if (!_configuration.Contains(subject))
                throw new ArgumentException("Target subject must be part of given configuration.");
            _target = subject;
            return this;
        }

        /// <summary>
        /// The parameter to restrict the results.  
        /// Setting this multiple times will simply use the last call.
        /// </summary>
        public virtual SqlGenerator WithWhere(ISqlParameter where)
        {
            // should we check that all fields are part of subjects in the current configuration?
            _where = where;
            return this;
        }

        /// <summary>
        /// Define a column to sort by.  Any additional joins that are required to use these columns will be included.
        /// Can be called multiple times.
        /// </summary>
        public virtual SqlGenerator OrderBy(IFieldPath path, SortDirection direction)
        {
            _orderBy.Add(new OrderedField(path, (direction == SortDirection.Ascending ? "ASC" : "DESC")));
            return this;
        }

        /// <summary>
        /// Define a column to group by.  Any additional joins that are required to use these columns will be included.
        /// Can be called multiple times.
        /// </summary>
        public virtual SqlGenerator GroupBy(IFieldPath path)
        {
            _groupBy.Add(path);
            return this;
        }

        /// <summary>
        /// Uses a column in the select list and also group by it.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual SqlGenerator ColumnGroupBy(IFieldPath path)
        {
            Column(path);
            GroupBy(path);
            return this;
        }

        /// <summary>
        /// Uses a column in the select list and also sort by it.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual SqlGenerator ColumnOrderBy(IFieldPath path, SortDirection direction)
        {
            Column(path);
            OrderBy(path, direction);
            return this;
        }

        /// <summary>
        /// Use a column in the select list, sort and group by it too.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public virtual SqlGenerator ColumnOrderByGroupBy(IFieldPath path, SortDirection direction)
        {
            Column(path);
            OrderBy(path, direction);
            GroupBy(path);
            return this;
        }

        public virtual void Validate()
        {
            if (_columns.Count == 0)
                throw new InvalidOperationException("No columns specified.");
            if (_target == null)
                throw new InvalidOperationException("No target specified.");
            if (_target.IdField == null)
                throw new InvalidOperationException("Target IdField is null.");

            // all fields in all parameters and columns must be linked to an ISqlSubject for this generator to operate correctly
            var invalid = new List<IField>();
            var paths = new List<IFieldPath>(_columns);
            paths.AddRange(_where.ToSqlString().Flatten().Parts.FindAll(o => o is IFieldPath).ConvertAll<IFieldPath>(o => (IFieldPath)o));
            foreach (var path in _columns)
                foreach (var f in path)
                    if (!(f.Subject is ISqlSubject))
                        invalid.Add(f);
            if (invalid.Count > 0)
                throw new InvalidOperationException("All fields must relate to ISqlSubjects.");
        }

        /// <summary>
        /// Generate a command using the current columns, target and parameters
        /// </summary>
        /// <param name="dbCommandType"></param>
        /// <returns></returns>
        public virtual void UpdateCommand(IDbCommand cmd)
        {
            Validate();

            // compile all required field paths; this will ensure every table we need to hit will be present
            List<IFieldPath> paths = new List<IFieldPath>();
            paths.Add(new FieldPath(_target.IdField));
            paths.AddRange(_columns);
            paths.AddRange(_orderBy.ConvertAll<IFieldPath>(s => s.Path));
            paths.AddRange(_groupBy);

            SqlString where = null;
            if (_where != null)
            {
                where = _where.ToSqlString().Flatten();
                paths.AddRange(where.Parts.FindAll(o => o is IFieldPath).ConvertAll<IFieldPath>(o => (IFieldPath)o));
            }

            // add all field paths and calculate unique joins for all of them
            var aliases = UniqueFieldPaths.FromPaths(paths);
            AssignAliases(aliases);

            var sb = new StringBuilder();
            sb.AppendFormat("SELECT {0} FROM {1}", GetColumnDefinitions(aliases), GetFromClause(aliases));
            cmd.Parameters.Clear();
            if (where != null)
            {
                // loop over parts of Where clause
                // if field, lookup alias and concat
                // if string, just concat
                // if parameter, add placeholder text, then add paramater to Command
                var whereStr = new StringBuilder();
                int paramIdx = 0;
                foreach (var part in where.Parts)
                {
                    if (part is IFieldPath)
                        whereStr.AppendFormat("{0}.[{1}]", aliases[(IFieldPath)part].Alias, ((IFieldPath)part).Last.SourceName);
                    else if (part is SqlStringParameter)
                    {
                        var p = (SqlStringParameter)part;
                        var cmdp = cmd.CreateParameter();
                        cmdp.ParameterName = String.Concat("@", paramIdx++);
                        cmdp.Value = p.Value;
                        cmd.Parameters.Add(cmdp);
                        whereStr.Append(cmdp.ParameterName);
                    }
                    else
                        whereStr.Append(part);
                }

                if (whereStr.Length > 0)
                {
                    sb.Append(" WHERE ");
                    sb.Append(whereStr);
                }
            }

            sb.Append(GetOrderByClause(aliases));
            sb.Append(GetGroupByClause(aliases));
            cmd.CommandText = sb.ToString();
        }

        private void AssignAliases(UniqueFieldPaths paths)
        {
            int i = 1;
            foreach (var path in paths)
            {
                var queue = new Queue<UniqueFieldPath>();
                queue.Enqueue(path);
                while (queue.Count > 0)
                {
                    var fields = queue.Dequeue();
                    fields.Alias = String.Concat("q", i++);
                    foreach (var f in fields)
                        queue.Enqueue(f);
                }
            }
        }

        /// <summary>
        /// Returns a string "field1, field2, field3"
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        protected virtual string GetColumnDefinitions(UniqueFieldPaths paths)
        {
            var sb = new StringBuilder();
            foreach (var c in _columns)
                sb.AppendFormat(String.Concat("{0}.[{1}]", (AliasColumns ? " AS [{2}]" : string.Empty), ", "), 
                    paths[c].Alias, c.Last.SourceName, c.Description);
            return sb.Remove(sb.Length - 2, 2).ToString();
        }

        /// <summary>
        /// Returns a string "table1 join table2 on x = y join table3 on a = b"
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        protected virtual string GetFromClause(UniqueFieldPaths paths)
        {
            // Target subject will be q0
            // All other subjects aliased from q1 up
            var sb = new StringBuilder();
            sb.AppendFormat("({0}) AS {1} ", _target.Sql, paths[new FieldPath(_target.IdField)].Alias);
            foreach (var ufp in paths)
            {
                // if this isn't part of our original target, join through the matrix
                if (!ufp.Subject.Equals(_target))
                {
                    // else top level subject not included - join with matrix
                    sb.AppendFormat("LEFT OUTER JOIN ({1}) AS m{2} ON m{2}.FromID = {0}.[{4}] LEFT OUTER JOIN ({3}) AS {2} ON m{2}.ToID = {2}.[{5}] ",
                        paths[new FieldPath(_target.IdField)].Alias,
                        _configuration[_target, ufp.Subject].Query,
                        ufp.Alias,
                        ufp.Subject.Sql,
                        _target.IdField.SourceName,
                        ufp.Subject.IdField.SourceName);
                }

                // ok so the first part of UniqueFieldPath is all good, now join the rest
                var queue = new Queue<UniqueFieldPath>();
                foreach (var f in ufp)
                    queue.Enqueue(f);

                while (queue.Count > 0)
                {
                    var field = queue.Dequeue();
                    sb.AppendFormat("LEFT OUTER JOIN ({0}) AS {1} ON {1}.[{4}] = {2}.[{3}] ",
                        ((ISqlSubject)field.Subject).Sql,
                        field.Alias,
                        field.Predecessor.Alias,
                        field.Field.SourceName,
                        field.Subject.IdField.SourceName);

                    foreach (var child in field)
                        queue.Enqueue(child);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a string "SORT BY column1 asc, column2 desc"
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        protected virtual string GetOrderByClause(UniqueFieldPaths paths)
        {
            var sb = new StringBuilder();
            foreach (var s in _orderBy)
                sb.AppendFormat("{0}.[{1}] {2}, ", paths[s.Path].Alias, s.Path.Last.SourceName, s.Direction);

            if (sb.Length == 0)
                return null;

            sb.Insert(0, " ORDER BY ");
            return sb.Remove(sb.Length - 2, 2).ToString();
        }

        /// <summary>
        /// Returns a string "GROUP BY column1, column2"
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        protected virtual string GetGroupByClause(UniqueFieldPaths paths)
        {
            var sb = new StringBuilder();
            foreach (var g in _groupBy)
                sb.AppendFormat("{0}.[{1}], ", paths[g].Alias, g.Last.SourceName);

            if (sb.Length == 0)
                return null;

            sb.Insert(0, " GROUP BY ");
            return sb.Remove(sb.Length - 2, 2).ToString();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
