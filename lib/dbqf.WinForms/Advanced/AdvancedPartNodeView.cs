using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dbqf.Display.Advanced;

namespace dbqf.WinForms.Advanced
{
    public partial class AdvancedPartNodeView : UserControl
    {
        private WinFormsAdvancedPartNode _adapter;
        public AdvancedPartNodeView(WinFormsAdvancedPartNode adapter)
        {
            InitializeComponent();
            _adapter = adapter;
            bsNode.DataSource = _adapter;

            lblPrefix.Click += Control_Click;
            lblDescription.Click += Control_Click;
            layoutMain.Click += Control_Click;
        }

        void Control_Click(object sender, EventArgs e)
        {
            _adapter.Select();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            _adapter.Delete();
        }
    }
}
