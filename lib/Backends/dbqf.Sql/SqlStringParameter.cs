using dbqf.Configuration;
using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Sql
{
    public class SqlStringParameter
    {
        public virtual object Value { get; set; }

        public SqlStringParameter()
        {
        }

        public SqlStringParameter(object value)
            : this()
        {
            Value = value;
        }

        public override string ToString()
        {
            return "?";
        }
    }
}
