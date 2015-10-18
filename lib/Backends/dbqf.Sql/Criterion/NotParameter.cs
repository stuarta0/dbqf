using dbqf.Criterion;
using System.Diagnostics;

namespace dbqf.Sql.Criterion
{
    [DebuggerDisplay("NOT ({_other})")]
    public class SqlNotParameter : NotParameter, ISqlParameter
    {
        public SqlNotParameter(ISqlParameter p)
            : base(p) { }

        public SqlString ToSqlString()
        {
            return new SqlString()
                .Add("NOT (")
                .Add(((ISqlParameter)Parameter).ToSqlString())
                .Add(")");
        }
    }
}
