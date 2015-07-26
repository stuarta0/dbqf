using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dbqf.Configuration;
using dbqf.Display;

namespace dbqf.WinForms
{
    public partial class FieldPathCombo : UserControl
    {
        private FieldPathComboAdapter _adapter;
        public FieldPathComboAdapter Adapter 
        {
            get { return _adapter; }
            set
            {
                if (_adapter != null)
                    _adapter.Items.ListChanged -= Items_ListChanged;

                _adapter = value;
                if (_adapter != null)
                {
                    _adapter.Items.ListChanged += Items_ListChanged;
                    Items_ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemAdded, 0));
                }
            }
        }

        public FieldPathCombo()
        {
            InitializeComponent();
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            if (this.DesignMode)
                layoutFieldPaths.Controls.Add(new Label() { Text = "(FieldPathCombo)", Dock = DockStyle.Fill });
        }

        void Items_ListChanged(object sender, ListChangedEventArgs e)
        {
            // find at what point the BindingSources changed
            int from = 0;
            for (; from < layoutFieldPaths.Controls.Count && from < Adapter.Items.Count && ((ComboBox)layoutFieldPaths.Controls[from]).DataSource == Adapter.Items[from].Fields; from++) ;

            for (int i = layoutFieldPaths.Controls.Count - 1; i >= from; i--)
            {
                var cbo = (ComboBox)layoutFieldPaths.Controls[i];
                cbo.SelectedIndexChanged -= cboFields_SelectedIndexChanged;
                layoutFieldPaths.Controls.Remove(cbo);
                layoutFieldPaths.RowStyles.RemoveAt(i);
                cbo.Dispose();
            }
            for (int i = from; i < Adapter.Items.Count; i++)
            {
                var cbo = new ComboBox();
                cbo.DropDownStyle = ComboBoxStyle.DropDownList;
                cbo.Anchor = AnchorStyles.Left | AnchorStyles.Right;

                layoutFieldPaths.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                layoutFieldPaths.Controls.Add(cbo);

                var item = Adapter.Items[i];
                cbo.DisplayMember = "DisplayName";
                cbo.DataSource = item.Fields;
                //cbo.DataBindings.Add("SelectedItem", item, "SelectedField", false, DataSourceUpdateMode.OnPropertyChanged);
                cbo.SelectedItem = item.SelectedField;
                cbo.Tag = item;
                cbo.SelectedIndexChanged += cboFields_SelectedIndexChanged;
            }
        }

        private void cboFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO: should be able to do this simply with a binding on the ComboBox
            ((FieldPathComboItem)((ComboBox)sender).Tag).SelectedField = (IField)((ComboBox)sender).SelectedItem;
        }
    }
}
