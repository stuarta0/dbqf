using System;
using System.Collections.Generic;
namespace dbqf.Configuration
{
    public interface IFieldList : IList<object>
    {
        /// <summary>
        /// Gets or sets an SQL string which will define options for a field.  There can optionally be a column aliased 'ID'
        /// which matches the ID field of the containing subject and will be used to limit items in the list based on a
        /// field path to the containing field.  Only one other column may be present which will be used to
        /// combine with parameters to restrict results.
        /// </summary>
        string Source { get; set; }

        FieldListType Type { get; set; }
    }
}
