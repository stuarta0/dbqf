using dbqf.Criterion;
using System.Collections.Generic;

namespace Standalone.Core.Data
{
    public class ListCallback : DbServiceAsyncCallback<List<object>>
    {
        public IFieldPath Path { get; set; }
        public ListCallback(AsyncCallback<List<object>> callback, IFieldPath path)
            : base(callback)
        {
            Path = path;
        }
    }
}
