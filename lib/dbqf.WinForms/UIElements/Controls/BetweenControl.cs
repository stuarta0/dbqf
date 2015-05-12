using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace dbqf.WinForms.UIElements.Controls
{
    public partial class BetweenControl : UserControl
    {
        public BetweenControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the label that appears before any other controls.
        /// </summary>
        public string PrefixText
        {
            get { return lblPrefix.Text; }
            set
            {
                lblPrefix.Visible = !String.IsNullOrEmpty(value);
                lblPrefix.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the label that appears after all other controls.
        /// </summary>
        public string PostfixText
        {
            get { return lblPostfix.Text; }
            set
            {
                lblPostfix.Visible = !String.IsNullOrEmpty(value);
                lblPostfix.Text = value;
            }
        }

        /// <summary>
        /// Sets the first or 'from' control.
        /// </summary>
        public Control Control1
        {
            set { SetControl(value, 1); }
        }

        /// <summary>
        /// Sets the second or 'to' control.
        /// </summary>
        public Control Control2
        {
            set { SetControl(value, 3); }
        }

        //private void SetLabel(string text, int colIndex)
        //{
        //    var label = new Label();
        //    label.Text = text;
        //    label.AutoSize = true;
        //    SetControl(label, colIndex);
        //}

        private void SetControl(Control control, int colIndex)
        {
            var existing = layout.GetControlFromPosition(colIndex, 0);
            if (existing != null)
                layout.Controls.Remove(existing);

            // WinForms AutoSize/GrowAndShrink logic is a bitch; at minimum size it will actually take the 
            // original Size (not MinimumSize) as the smallest a control can go (even though the Size 
            // property changes on resize, there's still an internal representation going on in there)
            control.Size = new Size(1, 1);
            control.Dock = DockStyle.Fill;
            layout.Controls.Add(control, colIndex, 0);
        }
    }
}
