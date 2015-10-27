using System.Data;

namespace Standalone.Core.Data
{
    public class ResultCallback : DbServiceAsyncCallback<DataTable>
    {
        public SearchDetails Details { get; set; }
        public ResultCallback(AsyncCallback<DataTable> callback, SearchDetails details)
            : base(callback)
        {
            Details = details;
        }
    }
}
