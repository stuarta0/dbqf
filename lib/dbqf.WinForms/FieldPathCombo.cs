using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dbqf.Configuration;

namespace dbqf.WinForms
{
    public partial class FieldPathCombo : UserControl
    {
        public FieldPathComboAdapter Adapter { get; private set; }
        public FieldPathCombo(FieldPathComboAdapter adapter)
        {
            InitializeComponent();
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

            Adapter = adapter;
            Adapter.ComboSource.ListChanged += ComboSource_ListChanged;
        }

        void ComboSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            // find at what point the BindingSources changed
            int from = 0;
            for (; from < layoutFieldPaths.Controls.Count && from < Adapter.ComboSource.Count && ((ComboBox)layoutFieldPaths.Controls[from]).DataSource == Adapter.ComboSource[from]; from++) ;

            for (int i = layoutFieldPaths.Controls.Count - 1; i >= from; i--)
            {
                var cbo = (ComboBox)layoutFieldPaths.Controls[i];
                cbo.SelectedIndexChanged -= cboFields_SelectedIndexChanged;
                layoutFieldPaths.Controls.Remove(cbo);
                layoutFieldPaths.RowStyles.RemoveAt(i);
                cbo.Dispose();
            }
            for (int i = from; i < Adapter.ComboSource.Count; i++)
            {
                var cbo = new ComboBox();
                cbo.DropDownStyle = ComboBoxStyle.DropDownList;
                cbo.DisplayMember = "DisplayName";
                cbo.Anchor = AnchorStyles.Left | AnchorStyles.Right;

                layoutFieldPaths.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                layoutFieldPaths.Controls.Add(cbo);

                cbo.DataSource = Adapter.ComboSource[i];
                cbo.SelectedItem = Adapter.SelectedPath[i];
                cbo.SelectedIndexChanged += cboFields_SelectedIndexChanged;
            }
        }

        private void cboFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            var replaceFrom = layoutFieldPaths.Controls.IndexOf((Control)sender);
            var path = Adapter.SelectedPath[0, replaceFrom];
            path.Add((IField)((ComboBox)sender).SelectedItem);
            Adapter.SelectedPath = path;
        }
    }
}
