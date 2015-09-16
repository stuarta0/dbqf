using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace dbqf.WinForms
{
    public partial class MultiPartTextBox : UserControl
    {
        public MultiPartTextBoxAdapter Adapter
        {
            get { return _adapter; }
            set
            {
                _adapter = value;

                if (_adapter == null)
                    bsAdapter.DataSource = typeof(MultiPartTextBoxAdapter);
                else
                    bsAdapter.DataSource = _adapter;
            }
        }
        private MultiPartTextBoxAdapter _adapter;

        public MultiPartTextBox()
        {
            InitializeComponent();
        }

        private void txtValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                _adapter.Search();
        }
    }
}
