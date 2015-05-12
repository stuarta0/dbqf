using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Standalone.Export
{
    public class CsvExportService : BaseExportService
    {
        protected override string Delimiter
        {
            get { return ","; }
        }
    }
}
