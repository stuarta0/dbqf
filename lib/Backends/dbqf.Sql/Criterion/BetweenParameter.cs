using dbqf.Configuration;
using dbqf.Criterion;

namespace dbqf.Sql.Criterion
{
    public class SqlBetweenParameter : BetweenParameter, ISqlParameter
    {
        public SqlBetweenParameter(IField field, object lo, object hi)
            : base(field, lo, hi) { }

        public SqlBetweenParameter(IFieldPath path, object lo, object hi)
            : base(path, lo, hi) { }

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
