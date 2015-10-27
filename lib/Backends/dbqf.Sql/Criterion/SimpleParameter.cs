using dbqf.Configuration;
using dbqf.Criterion;

namespace dbqf.Sql.Criterion
{
    public class SqlSimpleParameter : SimpleParameter, ISqlParameter
    {
        public SqlSimpleParameter(IField field, string op, object value)
            : base(field, op, value) { }

        public SqlSimpleParameter(IFieldPath path, string op, object value)
            : base(path, op, value) { }

        public SqlString ToSqlString()
        {
            return new SqlString()
                .AddField(Path)
                .Add(Operator)
                .AddParameter(Value);
        }
    }
}
