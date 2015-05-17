using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dbqf.Criterion;

namespace Standalone.Forms
{
    public partial class RetrieveFieldsView : UserControl
    {
        public RetrieveFieldsViewAdapter Adapter { get; private set; }
        public RetrieveFieldsView(RetrieveFieldsViewAdapter adapter)
        {
            InitializeComponent();

            Adapter = adapter;
            bsAdapter.DataSource = adapter;

            // initialise the tree
            foreach (var n in Adapter.GetChildren())
                tree.Nodes.Add(Create(n));
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Adapter.Fields.Clear();
        }

        #region Tree behaviour

        private void tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Action == TreeViewAction.Expand)
            {
                if (e.Node.Nodes[0] is TempNode)
                {
                    e.Node.Nodes.Clear();
                    foreach (var n in Adapter.GetChildren(e.Node.Tag as RetrieveFieldsViewAdapter.Node))
                        e.Node.Nodes.Add(Create(n));
                }
            }
        }

        private class TempNode : TreeNode { }
        private TreeNode Create(RetrieveFieldsViewAdapter.Node n)
        {
            var tn = new TreeNode(n.Text) { Tag = n };
            if (n.HasChildren)
                tn.Nodes.Add(new TempNode());
            return tn;
        }

        private void tree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Add(e.Node.Tag as RetrieveFieldsViewAdapter.Node);
        }

        private void Add(RetrieveFieldsViewAdapter.Node node)
        {
            var added = Adapter.Add(node);
            lstCustom.SelectedItems.Clear();
            foreach (var f in added)
                lstCustom.SelectedItems.Add(f);
        }

        #endregion

        #region Drag-drop behaviour

        private void tree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            tree.DoDragDrop(((TreeNode)e.Item).Tag as RetrieveFieldsViewAdapter.Node, DragDropEffects.Move);
        }

        private void lstCustom_DragEnter(object sender, DragEventArgs e)
        {
            // apparently GetData doesn't understand inheritence
            if (e.Data.GetDataPresent(typeof(RetrieveFieldsViewAdapter.SubjectNode)) 
                || e.Data.GetDataPresent(typeof(RetrieveFieldsViewAdapter.FieldNode)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void lstCustom_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(RetrieveFieldsViewAdapter.SubjectNode)))
                Add(e.Data.GetData(typeof(RetrieveFieldsViewAdapter.SubjectNode)) as RetrieveFieldsViewAdapter.Node);
            else
                Add(e.Data.GetData(typeof(RetrieveFieldsViewAdapter.FieldNode)) as RetrieveFieldsViewAdapter.Node);
        }

        private void lstCustom_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.None)
            {
                lstCustom.DoDragDrop(new List<FieldPath>(lstCustom.SelectedItems.Cast<FieldPath>()), DragDropEffects.Move);
            }
        }

        private void tree_DragDrop(object sender, DragEventArgs e)
        {
            foreach (var f in (List<FieldPath>)e.Data.GetData(typeof(List<FieldPath>)))
                Adapter.Fields.Remove(f);
        }

        private void tree_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(List<FieldPath>)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        #endregion

        #region List keyboard interaction

        private void lstCustom_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                foreach (var f in new List<FieldPath>(lstCustom.SelectedItems.Cast<FieldPath>()))
                    Adapter.Fields.Remove(f);
            }
        }

        private void lstCustom_MouseEnter(object sender, EventArgs e)
        {
            lstCustom.Focus();
        }

        private void tree_MouseEnter(object sender, EventArgs e)
        {
            tree.Focus();
        }

        #endregion

    }
}
