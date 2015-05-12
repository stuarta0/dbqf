using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display.Advanced;

namespace dbqf.WinForms.Advanced
{
    public partial class AdvancedPartView : UserControl
    {
        private AdvancedPart<Control> _part;
        public AdvancedPartView(AdvancedPart<Control> part)
        {
            InitializeComponent();
            _part = part;
            _part.PropertyChanged += Part_PropertyChanged;
            bsAdvancedPart.DataSource = part;
            UpdateFieldCombos();
            CreateElement();
        }

        void Part_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // rebuild comboboxes for the field path
            if (e.PropertyName.Equals("SelectedPath"))
                UpdateFieldCombos();
            else if (e.PropertyName.Equals("UIElement"))
                CreateElement();
        }

        private void cboFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            var replaceFrom = layoutFieldPaths.Controls.IndexOf((Control)sender);
            _part.SelectedPath = _part.SelectedPath[0, replaceFrom] 
                + dbqf.Criterion.FieldPath.FromDefault((IField)((ComboBox)sender).SelectedItem);
        }

        private void UpdateFieldCombos()
        {
            int validUntil;
            for (validUntil = 0; validUntil < _part.SelectedPath.Count && validUntil < layoutFieldPaths.Controls.Count; validUntil++)
            {
                var cbo = (ComboBox)layoutFieldPaths.Controls[validUntil];
                if (!_part.SelectedPath[validUntil].Equals(cbo.SelectedItem))
                    break;
            }

            // remove any remaining combos from validUntil onwards, then add any more needed between validUntil to SelectedPath count
            layoutFieldPaths.SuspendLayout();
            for (int i = layoutFieldPaths.Controls.Count - 1; i >= validUntil; i--)
            {
                var cbo = (ComboBox)layoutFieldPaths.Controls[i];
                cbo.SelectedIndexChanged -= cboFields_SelectedIndexChanged;
                layoutFieldPaths.Controls.Remove(cbo);
                layoutFieldPaths.RowStyles.RemoveAt(i);
                cbo.Dispose();
            }
            for (int i = validUntil; i < _part.SelectedPath.Count; i++)
            {
                var cbo = new ComboBox();
                cbo.DataSource = _part.GetFieldSource(_part.SelectedPath[i]);
                cbo.SelectedItem = _part.SelectedPath[i];
                cbo.SelectedIndexChanged += cboFields_SelectedIndexChanged;

                cbo.DropDownStyle = ComboBoxStyle.DropDownList;
                cbo.DisplayMember = "DisplayName";
                cbo.Anchor = AnchorStyles.Left | AnchorStyles.Right;

                layoutFieldPaths.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                layoutFieldPaths.Controls.Add(cbo);
            }
            layoutFieldPaths.ResumeLayout();
        }

        private void CreateElement()
        {
            layout.SuspendLayout();
            var hadFocus = false;
            var existing = layout.GetControlFromPosition(1, 3);
            if (existing != null)
            {
                hadFocus = existing.ContainsFocus;
                layout.Controls.Remove(existing);
            }

            if (_part.UIElement != null)
            {
                var control = _part.UIElement.Element;
                control.Size = new Size(1, 1);
                control.Dock = DockStyle.Fill;
                //control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                layout.Controls.Add(control, 1, 3);
                //control.TabIndex = 3;

                // try to move focus to the new control by defering the focus call
                // (still doesn't work when tabbing into the control - only works via click)
                if (hadFocus)
                    this.BeginInvoke(new MethodInvoker(() => { control.Focus(); }));
            }

            layout.ResumeLayout();
        }
    }
}
