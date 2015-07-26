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
using dbqf.WinForms.Advanced;

namespace dbqf.WinForms
{
    public partial class AdvancedView : UserControl
    {
        private WinFormsAdvancedAdapter _adapter;
        public WinFormsAdvancedAdapter Adapter 
        {
            get { return _adapter; }
            set
            {
                if (_adapter != null)
                {
                    _adapter.RebuildRequired -= Adapter_RebuildRequired;
                    _adapter.PropertyChanged -= Adapter_PropertyChanged;
                }

                _adapter = value;
                if (_adapter != null)
                {
                    _adapter.RebuildRequired += Adapter_RebuildRequired;
                    _adapter.PropertyChanged += Adapter_PropertyChanged;
                    bsAdapter.DataSource = _adapter;
                    fieldPathCombo.Adapter = _adapter.FieldPathComboAdapter;
                    CreateElement();
                }
                else
                    bsAdapter.DataSource = typeof(WinFormsAdvancedAdapter);
            }
        }

        public AdvancedView()
        {
            InitializeComponent();
        }

        void Adapter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("UIElement"))
                CreateElement();
            else if (e.PropertyName.Equals("IsValueVisible"))
                lblValue.Visible = Adapter.IsValueVisible;
        }

        private void CreateElement()
        {
            layoutMain.SuspendLayout();
            var hadFocus = false;
            var existing = layoutMain.GetControlFromPosition(1, 3);
            if (existing != null)
            {
                hadFocus = existing.ContainsFocus;
                layoutMain.Controls.Remove(existing);
            }

            if (Adapter.UIElement != null)
            {
                var control = Adapter.UIElement.Element;
                control.Size = new Size(1, 1);
                control.Dock = DockStyle.Fill;
                control.TabIndex = 6;
                //control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                layoutMain.Controls.Add(control, 1, 3);
                //control.TabIndex = 3;

                //// try to move focus to the new control by defering the focus call
                //// (still doesn't work when tabbing into the control - only works via click)
                //if (hadFocus)
                //    this.BeginInvoke(new MethodInvoker(() => { control.Focus(); }));
            }

            layoutMain.ResumeLayout();
        }

        void Adapter_RebuildRequired(object sender, EventArgs e)
        {
            foreach (Control c in pnlParameters.Controls)
                c.Dispose();
            pnlParameters.Controls.Clear();

            if (Adapter.Part == null)
                return;

            if (Adapter.Part is WinFormsAdvancedPartJunction)
                pnlParameters.Controls.Add(new AdvancedPartJunctionView((WinFormsAdvancedPartJunction)Adapter.Part));
            else if (Adapter.Part is WinFormsAdvancedPartNode)
                pnlParameters.Controls.Add(new AdvancedPartNodeView((WinFormsAdvancedPartNode)Adapter.Part));
            else
                throw new Exception("Unknown part type when rebuilding AdvancedView UI.");
        }

        private void btnAnd_Click(object sender, EventArgs e)
        {
            Adapter.Add(JunctionType.Conjunction);
        }

        private void btnOr_Click(object sender, EventArgs e)
        {
            Adapter.Add(JunctionType.Disjunction);
        }
    }
}
