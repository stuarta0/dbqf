using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Sql
{
    class IncompleteConfigurationException : Exception
    {
        public IncompleteConfigurationException(string message)
            : base(message)
        { }
    }
}
