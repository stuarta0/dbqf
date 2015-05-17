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
        private FieldPathCombo _pathSelector;
        public AdvancedPartView(AdvancedPart<Control> part, FieldPathCombo pathSelector)
        {
            InitializeComponent();
            _part = part;
            _part.PropertyChanged += Part_PropertyChanged;
            bsAdvancedPart.DataSource = part;
            
            _pathSelector = pathSelector;
            _pathSelector.Dock = DockStyle.Fill;
            layout.Controls.Add(_pathSelector, 1, 1);

            // TODO: set this correctly
            _part.SelectedPath = FieldPath.FromDefault(_part.SelectedSubject.DefaultField);
            //CreateElement();
        }

        void Part_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // rebuild comboboxes for the field path
            if (e.PropertyName.Equals("SelectedPath"))
                _pathSelector.Adapter.SelectedPath = _part.SelectedPath;
            else if (e.PropertyName.Equals("UIElement"))
                ; // CreateElement();
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
