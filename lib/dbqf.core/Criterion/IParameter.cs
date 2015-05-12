using dbqf.Processing;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Criterion
{
    /// <summary>
    /// Essentially the equivalent of NHibernate ICriterion.
    /// Concrete classes represent replacement for SearchValue.
    /// </summary>
    public interface IParameter
    {
        SqlString ToSqlString();
    }
}
