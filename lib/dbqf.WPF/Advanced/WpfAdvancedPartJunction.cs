using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace dbqf.WPF.Advanced
{
    public class WpfAdvancedPartJunction : WpfAdvancedPart
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
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemChanged)
                Parts[e.NewIndex].Container = this;
        }
    }
}
