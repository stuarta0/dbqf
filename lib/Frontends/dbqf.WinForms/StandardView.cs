using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dbqf.WinForms.Standard;
using dbqf.Display.Standard;

namespace dbqf.WinForms
{
    public partial class StandardView : UserControl
    {
        public StandardAdapter<Control> Adapter
        {
            get { return _adapter; }
        }

        private int _scrollHeight;
        protected int ScrollHeight
        {
            get { return _scrollHeight; }
            set
            {
                _scrollHeight = value;
                layout.AutoScrollMinSize = new System.Drawing.Size(0, _scrollHeight);
            }
        }

        private StandardAdapter<Control> _adapter;
        public StandardView(StandardAdapter<Control> adapter)
        {
            InitializeComponent();
            _adapter = adapter;
            _adapter.Parts.ListChanged += Parts_ListChanged;
            for (int i = 0; i < _adapter.Parts.Count; i++)
                AddPart(_adapter.Parts[i], i);

            _scrollHeight = (layout.Padding.Vertical + layout.Margin.Vertical) + ControlHeight(lnkAdd);
            layout.Padding = new Padding(layout.Padding.Left, layout.Padding.Top, SystemInformation.VerticalScrollBarWidth + layout.Padding.Right, layout.Padding.Bottom);
        }

        private int ControlHeight(Control c)
        {
            return c.Height + c.Padding.Vertical + c.Margin.Vertical;
        }

        void Parts_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
                AddPart(_adapter.Parts[e.NewIndex], e.NewIndex);
            else if (e.ListChangedType == ListChangedType.ItemDeleted)
                RemovePart(e.NewIndex);
        }

        private void AddPart(StandardPart<Control> part, int index)
        {
            // we're going to be internally consistent
            layout.SuspendLayout();
            var control = new StandardPartView(part);
            control.Dock = DockStyle.Fill;
            layout.RowStyles.Insert(index, new RowStyle(SizeType.AutoSize));

            // shift all controls after this point down one row
            // (the last row will have nothing in it, the row of index will have an existing control
            for (int i = layout.RowStyles.Count - 2; i >= index; i--)
            {
                var move = layout.GetControlFromPosition(0, i);
                if (move != null)
                    layout.SetCellPosition(move, new TableLayoutPanelCellPosition(0, i + 1));
            }

            layout.Controls.Add(control, 0, index);
            layout.ResumeLayout();

            // update height outside SuspendLayout or it won't recalculate correctly
            ScrollHeight += ControlHeight(control);
        }

        private void RemovePart(int index)
        {
            layout.SuspendLayout();
            var control = layout.GetControlFromPosition(0, index);
            layout.Controls.Remove(control);

            // shift all controls after this point up one row
            for (int i = index + 1; i < layout.RowStyles.Count; i++)
            {
                var move = layout.GetControlFromPosition(0, i);
                if (move != null)
                    layout.SetCellPosition(move, new TableLayoutPanelCellPosition(0, i - 1));
            }
            
            // the rowstyles are all identical, just remove the last
            layout.RowStyles.RemoveAt(layout.RowStyles.Count - 1); 
            control.Dispose();
            layout.ResumeLayout();

            // update height (and autoscroll size) outside SuspendLayout or it won't recalculate correctly
            ScrollHeight -= ControlHeight(control);
        }

        private void lnkAdd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _adapter.AddPart();
        }
    }
}
