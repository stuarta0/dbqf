using dbqf.Criterion;
using dbqf.Sql;
using System;
using System.Diagnostics;

namespace dbqf
{
    [DebuggerDisplay("{FieldPath} {SortDirection}")]
    public class OrderedField
    {
        public OrderedField(IFieldPath fieldPath)
        {
            FieldPath = fieldPath;
            SortDirection = SortDirection.Ascending;
        }

        public OrderedField(IFieldPath fieldPath, SortDirection sort)
            : this(fieldPath)
        {
            SortDirection = sort;
        }

        /// <summary>
        /// Gets the field path to sort on.
        /// </summary>
        public IFieldPath FieldPath { get; private set; }

        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        public SortDirection SortDirection { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", FieldPath, SortDirection);
        }
    }
}

