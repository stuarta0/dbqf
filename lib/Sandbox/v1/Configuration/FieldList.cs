using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;

namespace DbQueryFramework_v1.Configuration
{
    /// <summary>
    /// Allows a list of data to be stored for a configuration field as either source SQL or key/value pairs.
    /// </summary>
    [DebuggerDisplay("{Type}: {Source}")]
    public class FieldList : ICloneable
    {
        /// <summary>
        /// Gets or sets the source SQL for retrieving list items.  Must have at least a field named [Value] and optionally a field named [Display].  If [Display] isn't specified, then the data in the [Value] field will be used for display purposes.
        /// </summary>
        [XmlElement]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the items available for selection.
        /// </summary>
        [XmlArray]
        public List<FieldListItem> Items { get; set; }

        /// <summary>
        /// Gets or sets how the list items will be used for a field.  FieldListItem.None means ignore the list.
        /// </summary>
        [XmlAttribute]
        public FieldListType Type { get; set; }

        /// <summary>
        /// Initialises a new instance of the class.
        /// </summary>
        public FieldList()
        {
            Type = FieldListType.None;
        }

        /// <summary>
        /// Performs a deep clone of this object.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var l = new FieldList();
            l.Source = Source;
            l.Type = Type;

            if (Items != null)
            {
                l.Items = new List<FieldListItem>();
                foreach (var i in Items)
                    l.Items.Add((FieldListItem)i.Clone());
            }

            return l;
        }
    }
}
