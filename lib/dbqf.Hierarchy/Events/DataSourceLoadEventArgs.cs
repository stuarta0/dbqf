using dbqf.Configuration;
using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dbqf.Hierarchy.Events
{
    /// <summary>
    /// Allows interception of a DataSource.GetData call to modify the parameters or cancel the request.
    /// </summary>
    public class DataSourceLoadEventArgs : EventArgs
    {
        public ISubject Target { get; set; }
        public IList<IFieldPath> Fields { get; set; }
        public IParameter Where { get; set; }

        /// <summary>
        /// Gets or sets whether to cancel the call to IDataSource.GetData.
        /// </summary>
        public bool Cancel { get; set; }

        ///// <summary>
        ///// Indicates that this event has been dealt with and no longer requires bubbling.
        ///// </summary>
        //public bool IsHandled { get; set; }

        public DataSourceLoadEventArgs(ISubject target, IList<IFieldPath> fields, IParameter where)
            : base()
        {
            Target = target;
            Fields = fields;
            Where = where;

            Cancel = false;
        }
    }
}
