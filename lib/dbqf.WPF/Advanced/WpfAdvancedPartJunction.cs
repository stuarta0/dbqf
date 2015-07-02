using dbqf.Criterion;
using dbqf.Display;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace dbqf.WPF.Advanced
{
    [DebuggerDisplay("Junction {Parts.Count}")]
    public class WpfAdvancedPartJunction : WpfAdvancedPart, IPartViewJunction
    {
        public BindingList<WpfAdvancedPart> Parts { get; private set; }
        public JunctionType Type { get; set; }
        public string TypeName
        {
            get { return Type == JunctionType.Conjunction ? "AND" : "OR"; }
        }

        public WpfAdvancedPartJunction()
        {
            Type = JunctionType.Conjunction;
            Parts = new BindingList<WpfAdvancedPart>();
            Parts.ListChanged += Parts_ListChanged;
        }

        void Parts_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
                Parts[e.NewIndex].Container = this;
        }

        public override Criterion.IParameter GetParameter()
        {
            Junction junction = (Type == JunctionType.Conjunction ? (Junction)new Conjunction() : new Disjunction());
            foreach (var p in this)
                junction.Add(p.GetParameter());
            return junction;
        }

        #region IPartViewJunction

        public int IndexOf(IPartView item)
        {
            return Parts.IndexOf((WpfAdvancedPart)item);
        }

        public void Insert(int index, IPartView item)
        {
            Parts.Insert(index, (WpfAdvancedPart)item);
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
                Parts[index] = (WpfAdvancedPart)value;
            }
        }

        public void Add(IPartView item)
        {
            Parts.Add((WpfAdvancedPart)item);
        }

        public void Clear()
        {
            Parts.Clear();
        }

        public bool Contains(IPartView item)
        {
            return Parts.Contains((WpfAdvancedPart)item);
        }

        public void CopyTo(IPartView[] array, int arrayIndex)
        {
            Parts.CopyTo((WpfAdvancedPart[])array, arrayIndex);
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
            return Parts.Remove((WpfAdvancedPart)item);
        }

        public IEnumerator<IPartView> GetEnumerator()
        {
            return Parts.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
