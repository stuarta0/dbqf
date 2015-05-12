using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Initialisers
{
    /// <summary>
    /// Used to initialise application state on startup.
    /// </summary>
    public interface IInitialiser
    {
        void Initialise();
    }
}
