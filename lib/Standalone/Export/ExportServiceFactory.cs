using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Export
{
    public class ExportServiceFactory
    {
        public enum ExportType
        {
            CommaSeparated,
            TabDelimited
        }

        public IExportService Create(ExportType type)
        {
            if (type == ExportType.CommaSeparated)
                return new CsvExportService();
            else if (type == ExportType.TabDelimited)
                return new TabDelimitedExportService();

            throw new NotImplementedException();
        }
    }
}
