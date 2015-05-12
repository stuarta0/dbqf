using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;

namespace dbqf.Configuration
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
        Limited
    }

    /// <summary>
    /// Allows a list of data to be stored for a configuration field as either source SQL or key/value pairs.
    /// </summary>
    [DebuggerDisplay("{Type}: {Source}")]
    public class FieldList : IFieldList
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
        protected List<object> _items;

        /// <summary>
        /// Gets or sets how the list items will be used for a field.  FieldListItem.None means ignore the list.
        /// </summary>
        [XmlAttribute]
        public FieldListType Type { get; set; }

        public FieldList()
            : this(new object[0])
        {
        }

        public FieldList(IEnumerable<object> items)
        {
            Type = FieldListType.None;
            _items = new List<object>(items);
        }

        public int IndexOf(object item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, object item)
        {
            _items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        public object this[int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                _items[index] = value;
            }
        }

        public void Add(object item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(object item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(object item)
        {
            return _items.Remove(item);
        }

        public IEnumerator<object> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
