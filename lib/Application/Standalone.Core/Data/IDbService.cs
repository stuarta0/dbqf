using dbqf.Configuration;
using dbqf.Criterion;
using System.Collections.Generic;
using System.Data;

namespace Standalone.Core.Data
{
    public delegate void ResultCallback(ISearchDetails details, DataTable results);
    public delegate void ListCallback(IFieldPath path, List<string> results);

    public interface IDbService
    {
        DataTable GetResults(ISearchDetails details);
        List<string> GetList(IFieldPath path);
    }

    public interface IDbServiceAsync
    {
        void GetResults(ISearchDetails details, ResultCallback callback);
        void GetList(IFieldPath path, ListCallback callback);
    }
}
