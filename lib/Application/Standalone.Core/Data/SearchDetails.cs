using dbqf.Configuration;
using dbqf.Criterion;
using System.Collections.Generic;

namespace Standalone.Core.Data
{
    public class SearchDetails : ISearchDetails
    {
        public ISubject Target { get; set; }
        public IList<IFieldPath> Columns { get; set; }
        public IParameter Where { get; set; }
        public string Sql { get; set; }
    }
}
