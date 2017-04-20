using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dbqf.Criterion;
using dbqf.Sql;

namespace dbqf.Hierarchy
{
    public class GroupedField : OrderedField
    {
        public GroupedField(IFieldPath fieldPath) : base(fieldPath) { }
        public GroupedField(IFieldPath fieldPath, SortDirection sort) : base(fieldPath, sort) { }
        
        /// <summary>
        /// Gets or sets an overriding display format for the field, allowing grouping on the resulting format.
        /// </summary>
        public string DisplayFormat { get; set; }

        /// <summary>
        /// Gets or sets the text to use when a grouped value is null or empty.
        /// If null, defaults to $"(No {Field.DisplayName})"
        /// </summary>
        public string EmptyPlaceholder
        {
            get
            {
                if (_placeholder == null)
                    return $"(No {FieldPath?.Last.DisplayName})";
                return _placeholder;
            }
            set
            {
                _placeholder = value;
            }
        }
        private string _placeholder;
    }
}
