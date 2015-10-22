using dbqf.Configuration;
using dbqf.Criterion;
using System.Collections.Generic;

namespace Standalone.Core.Data
{
    public interface ISearchDetails
    {
        ISubject Target { get; }
        IList<IFieldPath> Columns { get; }
        IParameter Where { get; }
        string Sql { get; set; }
    }
}
