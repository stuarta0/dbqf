using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dbqf.Display.Standard;

namespace dbqf.WinForms.Standard
{
    public partial class StandardPartView : UserControl
    {
        private StandardPart<Control> _part;
        public StandardPartView(StandardPart<Control> part)
        {
            InitializeComponent();
            cboPath.Size = new Size(1, 1);
            cboBuilder.Size = new Size(1, 1);
            dbqf.WinForms.Extensions.ComboBoxExtension.AdjustWidthOnDropDown(cboPath, cboBuilder);

            _part = part;
            _part.PropertyChanged += Part_PropertyChanged;
            bsPart.DataSource = _part;
            CreateElement();
        }

        void Part_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("UIElement"))
                CreateElement();
        }

        private void CreateElement()
        {
            var hadFocus = false;
            var existing = layout.GetControlFromPosition(2, 0);
            if (existing != null)
            {
                hadFocus = existing.ContainsFocus;
                layout.Controls.Remove(existing);
            }

            if (_part.UIElement == null)
                return;

            var control = _part.UIElement.Element;
            control.Size = new Size(1, 1);
            control.Dock = DockStyle.Fill;
            layout.Controls.Add(control, 2, 0);
            control.TabIndex = 3;

            // try to move focus to the new control by defering the focus call
            // (still doesn't work when tabbing into the control - only works via click)
            if (hadFocus)
                this.BeginInvoke(new MethodInvoker(() => { control.Focus(); }));
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            _part.Remove();
        }

        //private void AdjustWidthComboBox_DropDown(object sender, System.EventArgs e)
        //{
        //    ComboBox combo = (ComboBox)sender;
        //    int width = combo.DropDownWidth;
        //    Graphics g = combo.CreateGraphics();
        //    Font font = combo.Font;

        //    int vertScrollBarWidth =
        //        (combo.Items.Count > combo.MaxDropDownItems) ? SystemInformation.VerticalScrollBarWidth : 0;

        //    int newWidth;
        //    foreach (var i in combo.Items)
        //    {
        //        newWidth = (int)g.MeasureString(combo.GetItemText(i), font).Width + vertScrollBarWidth;
        //        if (width < newWidth)
        //            width = newWidth;
        //    }

        //    combo.DropDownWidth = width;
        //}
    }
}
