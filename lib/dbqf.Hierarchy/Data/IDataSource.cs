using dbqf.Configuration;
using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace dbqf.Hierarchy.Data
{
    public interface IDataSource
    {
        DataTable GetData(ISubject target, IList<IFieldPath> fields, IParameter where);
        DataTable GetData(ISubject target, IList<IFieldPath> fields, IParameter where, IEnumerable<OrderedField> orderBy);
    }
}
