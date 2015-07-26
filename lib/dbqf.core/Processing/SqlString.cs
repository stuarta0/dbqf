using dbqf.Configuration;
using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Processing
{
    /// <summary>
    /// Will contain either FieldPath, string, SqlString, or SqlStringParameter.
    /// </summary>
    public class SqlString
    {
        // TODO: clean this up
        public List<object> Parts { get; private set; }

        public SqlString()
        {
            Parts = new List<object>();
        }

        public SqlString AddField(IFieldPath path)
        {
            Parts.Add(path);
            return this;
        }

        public SqlString AddField(IField field)
        {
            return AddField(new FieldPath(field));
        }

        public SqlString Add(string sql)
        {
            Parts.Add(sql);
            return this;
        }

        public SqlString Add(SqlString sql)
        {
            Parts.Add(sql);
            return this;
        }

        private SqlString Add(SqlStringParameter param)
        {
            Parts.Add(param);
            return this;
        }

        public SqlString AddParameter(object value)
        {
            return Add(new SqlStringParameter(value));
        }

        /// <summary>
        /// Takes any recursive SqlString parts and flattens them, resulting in an SqlString with only strings, fields and Parameters.
        /// </summary>
        /// <returns></returns>
        public SqlString Flatten()
        {
            var sql = new SqlString();
            Flatten(sql);
            return sql;
        }

        private void Flatten(SqlString cur)
        {
            // pull out all parts to the top level
            foreach (var p in Parts)
            {
                if (p is SqlString)
                    ((SqlString)p).Flatten(cur);
                else
                    cur.Parts.Add(p);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in Parts)
            {
                if (p is SqlStringParameter)
                    sb.Append(((SqlStringParameter)p).Value);
                else
                    sb.Append(p.ToString());
            }
            return sb.ToString();
        }
    }
}
