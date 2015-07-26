using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Core.Export
{
    public class ExportServiceFactory : IExportServiceFactory
    {
        protected Filter[] _filters;
        public ExportServiceFactory()
        {
            _filters = new Filter[] {
                new CsvFilter(),
                new TabFilter()
            };
        }

        #region Filter Classes

        protected abstract class ServiceFilter : Filter
        {
            public ServiceFilter(string name, string pattern)
                : base(name, pattern)
            {
            }

            public abstract IExportService CreateService();
        }

        protected class CsvFilter : ServiceFilter
        {
            public CsvFilter()
                : base("Comma-delimited (*.csv)", "csv")
            {
            }

            public override IExportService CreateService()
            {
                return new CsvExportService();
            }
        }

        protected class TabFilter : ServiceFilter
        {
            public TabFilter()
                : base("Tab-delimited (*.txt)", "txt")
            {
            }

            public override IExportService CreateService()
            {
                return new TabDelimitedExportService();
            }
        }

        #endregion

        #region IExportServiceFactory Implementation

        public virtual Filter[] GetFilters()
        {
            return _filters;
        }

        public virtual IExportService Create(Filter filter)
        {
            if (filter is ServiceFilter)
                return ((ServiceFilter)filter).CreateService();
            throw new NotImplementedException();
        }

        public virtual IExportService Create(string filename)
        {
            foreach (var filter in _filters)
                if (filter.IsMatch(filename))
                    return Create(filter);
            throw new NotImplementedException();
        }

        #endregion
    }
}
