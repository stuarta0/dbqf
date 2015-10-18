using dbqf.Criterion;
using dbqf.Sql;

namespace dbqf.Sql.Criterion
{
    /// <summary>
    /// Essentially the equivalent of NHibernate ICriterion.
    /// Concrete classes represent replacement for SearchValue.
    /// </summary>
    public interface ISqlParameter : IParameter
    {
        SqlString ToSqlString();
    }
}
