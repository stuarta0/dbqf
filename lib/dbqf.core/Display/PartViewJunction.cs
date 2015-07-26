using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Display
{
    /// <summary>
    /// Represents a collection of IPartViews.  Will not accept NULL in list of IPartViews.
    /// </summary>
    public class PartViewJunction : IPartViewJunction
    {
        public JunctionType Type { get; set; }
        private List<IPartView> _parts;

        public PartViewJunction()
        {
            Type = JunctionType.Conjunction;
            _parts = new List<IPartView>();
        }
        public PartViewJunction(JunctionType type)
            : this()
        {
            Type = type;
        }

        public void CopyFrom(IPartView other)
        {
            var junc = other as IPartViewJunction;
            if (junc != null)
            {
                _parts.Clear();
                Type = junc.Type;
                foreach (var p in junc)
                    Add(p);
            }
        }

        public IParameter GetParameter()
        {
            Junction junc = new Conjunction();
            if (Type == JunctionType.Disjunction)
                junc = new Disjunction();
            foreach (var v in this)
                junc.Add(v.GetParameter());
            return junc;
        }

        public bool Equals(IPartView other)
        {
            var junc = other as IPartViewJunction;
            if (junc != null)
            {
                if (this.Count == junc.Count)
                {
                    // ensure all elements are equal, otherwise false
                    for (int i = 0; i < this.Count; i++)
                        if (!this[i].Equals(junc[i]))
                            return false;

                    // all elements equal, so check type
                    return this.Type.Equals(junc.Type);
                }
                
                // different number of parts, therefore not equal
                return false;
            }
            return false;
        }

        public override string ToString()
        {
            if (Count == 0)
                return string.Empty;

            return String.Concat("(", String.Join(Type == JunctionType.Conjunction ? " and " : " or ",
                this.Convert<IPartView, string>(v => v.ToString()).ToArray()), ")");
        }

        #region IList<IPartView>

        public int IndexOf(IPartView item)
        {
            return _parts.IndexOf(item);
        }

        public void Insert(int index, IPartView item)
        {
            if (item == null)
                return;
            _parts.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _parts.RemoveAt(index);
        }

        public IPartView this[int index]
        {
            get
            {
                return _parts[index];
            }
            set
            {
                if (value != null)
                    _parts[index] = value;
            }
        }

        public void Add(IPartView item)
        {
            if (item == null)
                return;
            _parts.Add(item);
        }

        public void Clear()
        {
            _parts.Clear();
        }

        public bool Contains(IPartView item)
        {
            return _parts.Contains(item);
        }

        public void CopyTo(IPartView[] array, int arrayIndex)
        {
            _parts.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _parts.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(IPartView item)
        {
            return _parts.Remove(item);
        }

        public IEnumerator<IPartView> GetEnumerator()
        {
            return _parts.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
