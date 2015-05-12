using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Export
{
    public interface IExportService
    {
        void Export(string filename, DataTable data);
    }
}
