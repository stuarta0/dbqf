using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dbqf.Configuration;
using dbqf.Display.Preset;
using dbqf.Criterion;

namespace dbqf.WinForms
{
    public partial class PresetView : UserControl
    {
        public PresetAdapter<Control> Adapter
        {
            get { return _adapter; }
        }

        private Dictionary<Control, int> _heights;
        private PresetAdapter<Control> _adapter;
        public PresetView(PresetAdapter<Control> adapter)
        {
            InitializeComponent();
            _adapter = adapter;
            _adapter.PropertyChanged += Adapter_PropertyChanged;
            RebuildLayout();
        }

        void Adapter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Parts"))
                RebuildLayout();
        }

        private void RebuildLayout()
        {
            TableLayoutPanel table = null;
            if (this.Controls.Count > 0)
                table = this.Controls[0] as TableLayoutPanel;

            if (table != null)
            {
                table.SuspendLayout();
                foreach (Control c in table.Controls)
                {
                    c.SizeChanged -= Control_SizeChanged;
                    c.Dispose();
                }
                table.Dispose();
            }
            this.Controls.Clear();
            toolTip.RemoveAll();

            _heights = new Dictionary<Control, int>();
            table = new TableLayoutPanel();
            table.ColumnCount = 2;
            table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            table.Dock = DockStyle.Fill;
            table.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            table.AutoSize = true;
            table.AutoScroll = true;
            this.Controls.Add(table);
            table.SuspendLayout();

            table.RowCount = _adapter.Parts.Count + 1;
            for (int i = 0; i < _adapter.Parts.Count; i++)
            {
                var part = _adapter.Parts[i];
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                var label = new Label();
                label.AutoSize = true;
                label.Anchor = AnchorStyles.Left;
                label.Text = part.SelectedPath.Description.Replace("&", "&&");

                table.Controls.Add(label, 0, i);

                var control = part.UIElement.Element;
                control.Dock = DockStyle.Fill;
                table.Controls.Add(control, 1, i);
                toolTip.SetToolTip(control, part.SelectedPath.ToString());
                _heights.Add(control, 0);
                control.SizeChanged += Control_SizeChanged;
            }

            // add one more autosize row at the bottom to fill the remaining space
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            
            // HACK: table layout autoscroll causes a horizontal scrollbar to appear - offset by vertical scrollbar width fixes this (but pads out the UI and looks less than ideal)
            table.Padding = new Padding(table.Padding.Left, table.Padding.Top, SystemInformation.VerticalScrollBarWidth + table.Padding.Right, table.Padding.Bottom);

            table.ResumeLayout();
        }

        void Control_SizeChanged(object sender, EventArgs e)
        {
            var control = (Control)sender;
            var table = (TableLayoutPanel)control.Parent;

            // we do this here so controls have a chance to calculate layout before reporting their size has changed
            // (BetweenControl will only report correct size after being calculated for draw, not during add to parent control collection)
            int height = control.Height + control.Padding.Vertical + control.Margin.Vertical;
            if (_heights[control] != height)
            {
                _heights[control] = height;
                int total = 0;
                foreach (var kvp in _heights)
                    total += kvp.Value;

                table.AutoScrollMinSize = new Size(0, total);
            }
        }
    }
}
