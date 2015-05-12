using dbqf.Configuration;
using dbqf.Processing;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Criterion
{
    public class BetweenParameter : IParameter
    {
        private FieldPath _path;
        private object _lo;
        private object _hi;

        public BetweenParameter(IField field, object lo, object hi)
            : this(FieldPath.FromDefault(field), lo, hi)
        {
        }

        public BetweenParameter(FieldPath path, object lo, object hi)
        {
            _path = path;
            _lo = lo;
            _hi = hi;
        }

        public SqlString ToSqlString()
        {
            return new SqlString()
                .AddField(_path)
                .Add(" BETWEEN ")
                .AddParameter(_lo)
                .Add(" AND ")
                .AddParameter(_hi);
        }
    }
}
