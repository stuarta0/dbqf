using dbqf.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Criterion
{
    /// <summary>
    /// Represents a sequential path through a number of fields.  Will validate that the path is valid when fields are added to the list.
    /// Only the last item may be an IField.  All others must be IRelationField.
    /// </summary>
    public class FieldPath : dbqf.Criterion.IFieldPath
    {
        private List<IField> _fields;

        public ISubject Root
        {
            get 
            {
                if (_fields.Count > 0)
                    return _fields[0].Subject;
                return null;
            }
        }

        public IField Last
        {
            get
            {
                if (_fields.Count > 0)
                    return _fields[_fields.Count - 1];
                return null;
            }
        }

        /// <summary>
        /// Gets or sets a description describing this path.  If no description provided, it will be auto-calculated based on the current path.
        /// </summary>
        public virtual string Description 
        {
            get
            {
                if (!String.IsNullOrEmpty(_userDescription))
                    return _userDescription;

                if (_newDescriptionRequired)
                {
                    var sb = new StringBuilder(Root.DisplayName);
                    foreach (var f in _fields)
                        sb.AppendFormat(" {0}", f.DisplayName);
                    _autoDescription = sb.ToString().TrimStart();
                    _newDescriptionRequired = false;
                }

                return _autoDescription;
            }
            set
            {
                _userDescription = value;
            }
        }
        private string _userDescription;
        private string _autoDescription;
        private bool _newDescriptionRequired = true;

        public static FieldPath FromDefault(IField field)
        {
            var path = new FieldPath();
            var cur = field;
            while (cur is IRelationField)
            {
                path.Add(cur);
                cur = ((IRelationField)cur).RelatedSubject.DefaultField;
            }
            path.Add(cur);
            return path;
        }

        public FieldPath()
        {
            _fields = new List<IField>();
        }

        public FieldPath(params IField[] fields)
            : this((IEnumerable<IField>)fields)
        {
        }

        public FieldPath(IEnumerable<IField> fields)
            : this()
        {
            foreach (var f in fields)
                Add(f);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var f in _fields)
                sb.AppendFormat("{0} -> ", f.ToString());
            return sb.Remove(sb.Length - 4, 4).ToString();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 19;
                foreach (var f in _fields)
                    hash = hash * 31 + f.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is IFieldPath)
            {
                var other = (IFieldPath)obj;
                if (this.Count != other.Count)
                    return false;
                for (int i = 0; i < this.Count; i++)
                    if (this[i] != other[i])
                        return false;
                return true;
            }
            return base.Equals(obj);
        }

        public IFieldPath this[int from, int? to]
        {
            get
            {
                var f = new FieldPath();
                if (from < 0)
                    from = 0;
                if (!to.HasValue)
                    to = _fields.Count;
                else if (to < 0)
                    to += _fields.Count;

                for (int i = from; i < _fields.Count && i < to; i++)
                    f.Add(_fields[i]);
                return f;
            }
        }

        public static FieldPath operator +(FieldPath path1, FieldPath path2)
        {
            var path3 = new FieldPath(path1);
            path3.Add(path2);
            return path3;
        }

        public static FieldPath operator +(FieldPath path, IField field)
        {
            var path2 = new FieldPath(path);
            path2.Add(field);
            return path2;
        }

        public int IndexOf(IField item)
        {
            return _fields.IndexOf(item);
        }

        public void Insert(int index, IField item)
        {
            // make sure this field fits in after the previous field and before the next field
            if (index >= _fields.Count)
            {
                Add(item);
                return;
            }

            if (!(item is IRelationField))
                throw new ArgumentException(String.Format("Cannot insert field '{0}' because it is not an IRelationField.", item.SourceName));

            if (index > 0 && (!(_fields[index - 1] is RelationField) || ((IRelationField)_fields[index - 1]).RelatedSubject.Equals(item.Subject)))
                throw new ArgumentException(String.Format("Cannot insert field '{0}' at index {1} since the previous field '{2}' does not relate.", item.SourceName, index, _fields[index - 1].SourceName));

            if (!((IRelationField)item).RelatedSubject.Equals(_fields[index].Subject))
                throw new ArgumentException(String.Format("Cannot insert field '{0}' at index {1} since the next field '{2}' does not relate.", item.SourceName, index, _fields[index + 1].SourceName));

            _fields.Insert(index, item);
            _newDescriptionRequired = true;
        }

        public void RemoveAt(int index)
        {
            // guaranteed at this point that the previous items in the list will be IRelationField, but after it might not
            var item = _fields[index];
            if (index == 0 || index == _fields.Count - 1 || ((IRelationField)_fields[index - 1]).RelatedSubject.Equals(_fields[index + 1].Subject))
                _fields.Remove(item);
            else
                throw new ArgumentException(String.Format("Cannot remove field '{0}' since the previous field '{1}' does not relate to the following field '{2}'.", item.SourceName, _fields[index - 1].SourceName, _fields[index + 1].SourceName));
            _newDescriptionRequired = true;
        }

        public IField this[int index]
        {
            get
            {
                return _fields[index];
            }
            set
            {
                // ensure that replacing this index in the field path is valid
                if (index == 0 && _fields.Count > 1 && (!(value is IRelationField) || !((IRelationField)value).RelatedSubject.Equals(_fields[index + 1].Subject)))
                    throw new ArgumentException(String.Format("Replacing field '{0}' at index {1} with field '{2}' would create an invalid path to the next field '{3}'", _fields[index].SourceName, index, value.SourceName, _fields[index + 1].SourceName));
                    
                else if (index == _fields.Count - 1 && _fields.Count > 1 && !((IRelationField)_fields[_fields.Count - 2]).RelatedSubject.Equals(value.Subject))
                    throw new ArgumentException(String.Format("Replacing field '{0}' at index {1} with field '{2}' would create an invalid path from the last field '{3}'", _fields[index].SourceName, index, value.SourceName, _fields[index - 2].SourceName));

                else if (_fields.Count > 2 && (!(value is IRelationField) || !((IRelationField)_fields[index - 1]).RelatedSubject.Equals(value.Subject) || !((IRelationField)value).RelatedSubject.Equals(_fields[index + 1].Subject)))
                    throw new ArgumentException(String.Format("Replacing field '{0}' at index {1} with field '{2}' would create an invalid path from the last field '{3}' through this field to the next field '{4}'", _fields[index].SourceName, index, value.SourceName, _fields[index - 1].SourceName, _fields[index + 1].SourceName));

                _fields[index] = value;
                _newDescriptionRequired = true;
            }
        }

        /// <summary>
        /// Add another field path to this path.  Will ensure it's still a valid path from root to last.
        /// </summary>
        /// <param name="other"></param>
        public void Add(IFieldPath other)
        {
            foreach (var f in other)
                Add(f);
        }

        public void Add(IField item)
        {
            if (_fields.Count == 0)
                _fields.Add(item);
            else
            {
                var field = _fields[_fields.Count - 1];
                if (!(field is IRelationField))
                    throw new InvalidOperationException(String.Format("Cannot add another field '{0}' to FieldList since the last field '{1}' is not an IRelationField.", item.SourceName, field.SourceName));

                var relation = (IRelationField)field;
                if (!relation.RelatedSubject.Equals(item.Subject))
                    throw new ArgumentException(String.Format("Cannot add field '{0}' to FieldPath as it does not follow on from previous field '{1}'.", item.SourceName, field.SourceName));

                _fields.Add(item);
            }
            _newDescriptionRequired = true;
        }

        public void Clear()
        {
            _fields.Clear();
            _newDescriptionRequired = true;
        }

        public bool Contains(IField item)
        {
            return _fields.Contains(item);
        }

        public void CopyTo(IField[] array, int arrayIndex)
        {
            _fields.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _fields.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(IField item)
        {
            // ensure that the removal of the item doesn't break the valid path
            int index = _fields.IndexOf(item);
            if (index < 0)
                return false;

            RemoveAt(index);
            _newDescriptionRequired = true;
            return true;
        }

        public IEnumerator<IField> GetEnumerator()
        {
            return _fields.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
