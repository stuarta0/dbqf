using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Core.Data
{
    public class DbServiceAsyncCallback<T> : IDbServiceAsyncCallback<T>
    {
        public virtual T Results { get; set; }
        public virtual Exception Exception { get; set; }
        public virtual AsyncCallback<T> Callback { get; set; }

        public DbServiceAsyncCallback(AsyncCallback<T> callback)
        {
            Callback = callback;
        }
    }
}
