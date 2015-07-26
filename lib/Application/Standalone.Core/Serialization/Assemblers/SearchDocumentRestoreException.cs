using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Standalone.Core.Serialization.Assemblers
{
    class SearchDocumentRestoreException : Exception
    {
        public SearchDocumentRestoreException(string message)
            : base(message)
        { }
    }
}
