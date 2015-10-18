using dbqf.Configuration;
using dbqf.Criterion;

namespace dbqf.Sql.Criterion
{
    public class SqlNullParameter : NullParameter, ISqlParameter
    {
        public SqlNullParameter(IField field)
            : base(field) { }

        public SqlNullParameter(IFieldPath path)
            : base(path) { }

        public SqlString ToSqlString()
        {
            return new SqlString()
                .AddField(Path)
                .Add(" IS NULL");
        }
    }
}
