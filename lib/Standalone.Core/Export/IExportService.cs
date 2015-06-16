using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Core.Export
{
    public interface IExportService
    {
        /// <summary>
        /// Exports data to a file.
        /// </summary>
        /// <param name="filename">Export data to this file.</param>
        /// <param name="data">The data to export.</param>
        /// <returns>True if the file was opened by the service, false if the file is closed.</returns>
        bool Export(string filename, DataTable data);
    }
}
