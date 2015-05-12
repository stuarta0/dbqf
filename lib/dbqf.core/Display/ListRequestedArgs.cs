using dbqf.Configuration;
using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace dbqf.Display
{
    public delegate void ListRequestedEventHandler(object sender, ListRequestedArgs e);

    public class ListRequestedArgs : EventArgs
    {
        /// <summary>
        /// Gets the path that is requested to load list data.
        /// </summary>
        public FieldPath Path { get; protected set; }

        /// <summary>
        /// Gets or sets the type of list for this path.
        /// </summary>
        public FieldListType Type { get; set; }

        /// <summary>
        /// Gets or sets the list data for this path.
        /// </summary>
        public BindingList<object> List { get; set; }

        public ListRequestedArgs(FieldPath path)
        {
            Path = path;
        }
    }
}
