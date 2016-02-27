using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Display
{
    /// <summary>
    /// Event data containing the part that was recently created.
    /// </summary>
    public class PartCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the newly created part.
        /// </summary>
        public IPartViewNode Part { get; private set; }

        public PartCreatedEventArgs(IPartViewNode part)
        {
            Part = part;
        }
    }
}
