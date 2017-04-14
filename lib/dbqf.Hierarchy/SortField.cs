using dbqf.Configuration;
using System;
using System.Diagnostics;

namespace dbqf
{
    [DebuggerDisplay("{FieldSourceName} {SortDirection}")]
    public class SortField
    {
        public enum Direction { Ascending, Descending }

        /// <summary>
        /// Gets or sets the source name of the field.
        /// </summary>
        public IField Field { get; private set; }

        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        public Direction SortDirection { get; set; }

        public SortField(IField field)
        {
            Field = field;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Field.SourceName, SortDirection);
        }
    }
}

