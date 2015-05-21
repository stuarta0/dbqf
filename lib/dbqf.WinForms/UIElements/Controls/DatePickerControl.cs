using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dbqf.Display;

namespace dbqf.WinForms.UIElements.Controls
{
    public partial class DatePickerControl : UserControl
    {
        public DatePickerControl()
        {
            InitializeComponent();
        }

        private UIElement<Control> _other;
        public UIElement<Control> Other
        {
            get { return _other; }
            set 
            {
                if (_other != null)
                    layout.Controls.Remove(_other.Element);

                _other = value;

                // WinForms AutoSize/GrowAndShrink logic is a bitch; at minimum size it will actually take the 
                // original Size (not MinimumSize) as the smallest a control can go (even though the Size 
                // property changes on resize, there's still an internal representation going on in there)
                var control = _other.Element;
                control.Size = new Size(1, 1);
                control.Dock = DockStyle.Fill;
                layout.Controls.Add(control, 0, 0);
            }
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (Other != null)
            {
                object[] values = new object[] { dateTimePicker.Value };
                if (Other.Parser != null)
                    values = Other.Parser.Parse(values);
                Other.SetValues(values);
            }
        }
    }
}
