using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dbqf.Display.Advanced;
using dbqf.Display;

namespace dbqf.WinForms.Advanced
{
    public partial class AdvancedPartJunctionView : UserControl
    {
        private WinFormsAdvancedPartJunction _adapter;
        public AdvancedPartJunctionView(WinFormsAdvancedPartJunction adapter)
        {
            InitializeComponent();
            _adapter = adapter;
            _adapter.Parts.ListChanged += Parts_ListChanged;
            bsJunction.DataSource = _adapter;
            layoutParts.BorderStyle = BorderStyle.FixedSingle;

            // initialise parts
            for (int i = 0; i < _adapter.Parts.Count; i++)
                AddPart(_adapter.Parts[i], i);
        }

        void Parts_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
                AddPart(_adapter.Parts[e.NewIndex], e.NewIndex);
            else if (e.ListChangedType == ListChangedType.ItemDeleted)
                RemovePart(e.NewIndex);
        }

        private void AddPart(AdvancedPart part, int index)
        {
            // we're going to be internally consistent
            layoutParts.SuspendLayout();
            Control control;
            if (part is WinFormsAdvancedPartJunction)
                control = new AdvancedPartJunctionView((WinFormsAdvancedPartJunction)part);
            else
                control = new AdvancedPartNodeView((WinFormsAdvancedPartNode)part);

            control.Dock = DockStyle.Fill;
            layoutParts.RowStyles.Insert(index, new RowStyle(SizeType.AutoSize));

            // shift all controls after this point down one row
            // (the last row will have nothing in it, the row of index will have an existing control
            for (int i = layoutParts.RowStyles.Count - 2; i >= index; i--)
            {
                var move = layoutParts.GetControlFromPosition(0, i);
                if (move != null)
                    layoutParts.SetCellPosition(move, new TableLayoutPanelCellPosition(0, i + 1));
            }

            layoutParts.Controls.Add(control, 0, index);
            layoutParts.ResumeLayout();

            // update height outside SuspendLayout or it won't recalculate correctly
            //ScrollHeight += ControlHeight(control);
        }

        private void RemovePart(int index)
        {
            layoutParts.SuspendLayout();
            var control = layoutParts.GetControlFromPosition(0, index);
            layoutParts.Controls.Remove(control);

            // shift all controls after this point up one row
            for (int i = index + 1; i < layoutParts.RowStyles.Count; i++)
            {
                var move = layoutParts.GetControlFromPosition(0, i);
                if (move != null)
                    layoutParts.SetCellPosition(move, new TableLayoutPanelCellPosition(0, i - 1));
            }

            // the rowstyles are all identical, just remove the last
            layoutParts.RowStyles.RemoveAt(layoutParts.RowStyles.Count - 1);
            control.Dispose();
            layoutParts.ResumeLayout();

            // update height (and autoscroll size) outside SuspendLayout or it won't recalculate correctly
            //ScrollHeight -= ControlHeight(control);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            _adapter.Delete();
        }
    }
}
