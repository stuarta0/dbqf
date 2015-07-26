using System;
using System.Linq;

namespace Standalone.Core.Export
{
    public class Filter
    {
        public string Name { get; set; }
        public string[] Extensions { get; set; }

        /// <summary>
        /// Gets a file pattern based on the Extensions. For example, ["csv", "txt"] => "*.csv,*.txt"
        /// </summary>
        public string FilePattern
        {
            get { return String.Join(",", Extensions.Convert<string, string>(p => String.Concat("*.", p))); }
        }

        /// <summary>
        /// Creates a Filter instance.
        /// </summary>
        /// <param name="name">The name of the filter displayed to the user</param>
        /// <param name="extensions">The list of file extensions that match this filter object (without leading asterisk or stop).</param>
        public Filter(string name, params string[] extensions)
        {
            Name = name;
            Extensions = extensions;
        }

        public bool IsMatch(string filename)
        {
            return Extensions.Contains(System.IO.Path.GetExtension(filename).Substring(1));
        }

        public override string ToString()
        {
            return Name;
        }
    }

}
