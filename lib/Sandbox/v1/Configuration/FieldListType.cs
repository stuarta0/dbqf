using System;
using System.Collections.Generic;
using System.Text;

namespace DbQueryFramework_v1.Configuration
{
    /// <summary>
    /// Indicates how to treat a field that has list items in a UI element.
    /// </summary>
    public enum FieldListType
    {
        /// <summary>
        /// None.
        /// </summary>
        None,

        /// <summary>
        /// Suggests values but doesn't require a user to choose one.
        /// </summary>
        Suggested,

        /// <summary>
        /// Only allows values supplied in a list.
        /// </summary>
        LimitToList
    }
}
