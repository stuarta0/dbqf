using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Core.Export
{
    public class TabDelimitedExportService : BaseExportService
    {
        protected override string Delimiter
        {
            get { return "\t"; }
        }
    }
}
