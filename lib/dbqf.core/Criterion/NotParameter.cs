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
        IParameter _other;
        public NotParameter(IParameter other)
        {
            _other = other;
        }

        public SqlString ToSqlString()
        {
            return new SqlString()
                .Add("NOT (")
                .Add(_other.ToSqlString())
                .Add(")");
        }
    }
}
