using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Standalone.Forms
{
    public partial class RetrieveFieldsView : UserControl
    {
        public RetrieveFieldsViewAdapter Adapter { get; private set; }
        public RetrieveFieldsView(RetrieveFieldsViewAdapter adapter)
        {
            InitializeComponent();

            rdoPredefined.DataBindings["Checked"].Format += (sender, e) => 
            { 
                e.Value = ((RetrieveFieldsViewAdapter.Method)e.Value) == RetrieveFieldsViewAdapter.Method.Predefined; 
            };
            rdoCustom.DataBindings["Checked"].Format += (sender, e) => 
            { 
                e.Value = ((RetrieveFieldsViewAdapter.Method)e.Value) == RetrieveFieldsViewAdapter.Method.Custom; 
            };

            rdoPredefined.DataBindings["Checked"].Parse += (sender, e) => 
            { 
                if ((bool)e.Value)
                    e.Value = RetrieveFieldsViewAdapter.Method.Predefined;
            };
            rdoCustom.DataBindings["Checked"].Parse += (sender, e) =>
            {
                if ((bool)e.Value)
                    e.Value = RetrieveFieldsViewAdapter.Method.Custom;
            };

            Adapter = adapter;
            bsAdapter.DataSource = adapter;
        }
    }
}
