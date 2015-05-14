using dbqf.Configuration;
using dbqf.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace dbqf.Criterion
{
    [DebuggerDisplay("NOT ({_other})")]
    public class NotParameter : IParameter
    {
        public IParameter Parameter { get; private set; }

        public NotParameter(IParameter p)
        {
            Parameter = p;
        }

        public SqlString ToSqlString()
        {
            return new SqlString()
                .Add("NOT (")
                .Add(Parameter.ToSqlString())
                .Add(")");
        }
    }
}
