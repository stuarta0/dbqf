using System;
using System.ComponentModel;
using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Criterion.Builders;
using dbqf.Parsers;
using System.Collections.Generic;
using System.Diagnostics;

namespace dbqf.Display.Advanced
{
    /// <summary>
    /// Encapsulates logic relating to how to display a junction in the advanced search control.
    /// </summary>
    [DebuggerDisplay("{Type} Count={Parts.Count}")]
    public class AdvancedPartJunction : AdvancedPart, IPartViewJunction
    {
        public IParameterBuilderFactory Builder { get; set; }

        /// <summary>
        /// Construct a new AdvancedPartJunction that represents an IPartViewJunction.
        /// </summary>
        public AdvancedPartJunction()
            : this(JunctionType.Conjunction)
        {
        }

        /// <summary>
        /// Construct a new AdvancedPartJunction that represents an IPartViewJunction.
        /// </summary>
        /// <param name="type">The type of junction.</param>
        public AdvancedPartJunction(JunctionType type)
        {
            Type = type;
            Parts = new BindingList<AdvancedPart>();
            Parts.ListChanged += Parts_ListChanged;
        }
        
        public BindingList<AdvancedPart> Parts { get; private set; }
        public JunctionType Type { get; set; }
        public string TypeName
        {
            get { return Type == JunctionType.Conjunction ? "AND" : "OR"; }
        }

        void Parts_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
                Parts[e.NewIndex].Container = this;

            // TODO: inform the target that it's prefix may have changed - implies incorrect architecture if we need to do this
            //else if (e.ListChangedType == ListChangedType.ItemMoved)
            //    Parts[e.NewIndex].OnPropertyChanged("Prefix");
        }

        public override Criterion.IParameter GetParameter()
        {
            if (Builder == null)
                return null;

            IJunction junction = (Type == JunctionType.Conjunction ? Builder.Conjunction() : Builder.Disjunction());
            foreach (var p in this)
            {
                var toAdd = p.GetParameter();
                if (toAdd != null)
                    junction.Add(toAdd);
            }

            if (junction.Count == 0)
                return null;

            return junction;
        }

        #region IPartViewJunction

        public int IndexOf(IPartView item)
        {
            return Parts.IndexOf((AdvancedPart)item);
        }

        public void Insert(int index, IPartView item)
        {
            Parts.Insert(index, (AdvancedPart)item);
        }

        public void RemoveAt(int index)
        {
            Parts.RemoveAt(index);
        }

        public IPartView this[int index]
        {
            get
            {
                return Parts[index];
            }
            set
            {
                Parts[index] = (AdvancedPart)value;
            }
        }

        public void Add(IPartView item)
        {
            Parts.Add((AdvancedPart)item);
        }

        public void Clear()
        {
            Parts.Clear();
        }

        public bool Contains(IPartView item)
        {
            return Parts.Contains((AdvancedPart)item);
        }

        public void CopyTo(IPartView[] array, int arrayIndex)
        {
            Parts.CopyTo((AdvancedPart[])array, arrayIndex);
        }

        public int Count
        {
            get { return Parts.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(IPartView item)
        {
            return Parts.Remove((AdvancedPart)item);
        }

        public IEnumerator<IPartView> GetEnumerator()
        {
            foreach (IPartView part in Parts)
                yield return part;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
