using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Standalone.Core.Export
{
    public abstract class BaseExportService : IExportService
    {
        protected abstract string Delimiter { get; }

        private Regex replacer;
        public BaseExportService()
        {
            replacer = new Regex("(\\r\\n)|(\")");
        }

        public virtual void Export(string filename, DataTable data)
        {
            var contents = new StringBuilder();

            for (int i = 0; i < data.Columns.Count; i++)
            {
                contents.Append(data.Columns[i].ColumnName);
                contents.Append(Delimiter);
            }
            contents.Append("\r\n");

            foreach (DataRow row in data.Rows)
            {
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    contents.Append(Quote(row[i].ToString()));
                    contents.Append(Delimiter);
                }
                contents.Append("\r\n");
            }

            System.IO.File.WriteAllText(filename, contents.ToString());
        }

        public virtual string Quote(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return string.Empty;

            // http://stackoverflow.com/a/9512606
            // don't need to escape commas or tabs as the values are quoted
            return String.Concat("\"", replacer.Replace(value, m =>
            {
                if (!String.IsNullOrEmpty(m.Groups[1].Value))
                    return "\n";
                else
                    return "\"\"";
            }).Trim(), "\"");
        }
    }
}
