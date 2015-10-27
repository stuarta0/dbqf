
using dbqf.Configuration;
using dbqf.Criterion;
namespace dbqf.Sql.Criterion
{
    public class SqlLikeParameter : LikeParameter, ISqlParameter
    {
        public SqlLikeParameter(IField field, string value)
            : base(field, value) { }

        public SqlLikeParameter(IField field, string value, MatchMode mode)
            : base(field, value, mode) { }

        public SqlLikeParameter(IFieldPath path, string value)
            : base(path, value) { }

        public SqlLikeParameter(IFieldPath path, string value, MatchMode mode)
            : base(path, value, mode) { }

        public SqlString ToSqlString()
        {
            return new SqlString()
                .AddField(Path)
                .Add(" LIKE ")
                .AddParameter(Mode.ToMatchString(Value));
        }
    }
}
