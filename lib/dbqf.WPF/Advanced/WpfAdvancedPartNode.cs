using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dbqf.WPF.Advanced
{
    public class WpfAdvancedPartNode : WpfAdvancedPart
    {
        public WpfAdvancedPartNode()
        {
        }

        public string Prefix
        {
            get 
            {
                if (Container != null && Container.Parts.IndexOf(this) > 0)
                    return Container.TypeName;
                return string.Empty; 
            }
        }

        public string Description
        {
            get { return "hello world"; }
        }
    }
}
