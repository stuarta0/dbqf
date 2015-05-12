using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Diagnostics;

namespace DbQueryFramework_v1.Configuration
{
    /// <summary>
    /// Represents an item that could be chosen as the value for a field in a UI.
    /// </summary>
    [DebuggerDisplay("{Value} ({Id})")]
    public class FieldListItem : ICloneable
    {
        /// <summary>
        /// Gets or sets an ID that corresponds to the subject containing the field with this field list.  This allows options for a field to be filtered through ComplexField relationships.
        /// </summary>
        [XmlElement]
        public object Id { get; set; }

        /// <summary>
        /// Gets or sets the value to compare against the field.  String must be convertable to Field.DataType.  
        /// The only reason the type is a string is for serialisation purposes.
        /// </summary>
        [XmlElement]
        public object Value { get; set; }

        public FieldListItem()
        {
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
