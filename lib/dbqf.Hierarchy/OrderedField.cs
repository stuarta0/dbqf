using dbqf.Criterion;
using dbqf.Sql;
using System;
using System.Diagnostics;

namespace dbqf.Hierarchy
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

        public override bool Equals(object obj)
        {
            if (obj is OrderedField)
                return ((OrderedField)obj).FieldPath.Equals(this.FieldPath);
            return base.Equals(obj);
        }
    }
}

