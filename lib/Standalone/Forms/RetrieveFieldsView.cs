using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Standalone.Forms
{
    public partial class RetrieveFieldsView : UserControl
    {
        public RetrieveFieldsViewAdapter Adapter { get; private set; }
        public RetrieveFieldsView(RetrieveFieldsViewAdapter adapter)
        {
            InitializeComponent();

            rdoPredefined.DataBindings["Checked"].Format += (sender, e) => 
            { 
                e.Value = ((RetrieveFieldsViewAdapter.Method)e.Value) == RetrieveFieldsViewAdapter.Method.Predefined; 
            };
            rdoCustom.DataBindings["Checked"].Format += (sender, e) => 
            { 
                e.Value = ((RetrieveFieldsViewAdapter.Method)e.Value) == RetrieveFieldsViewAdapter.Method.Custom; 
            };

            rdoPredefined.DataBindings["Checked"].Parse += (sender, e) => 
            { 
                if ((bool)e.Value)
                    e.Value = RetrieveFieldsViewAdapter.Method.Predefined;
            };
            rdoCustom.DataBindings["Checked"].Parse += (sender, e) =>
            {
                if ((bool)e.Value)
                    e.Value = RetrieveFieldsViewAdapter.Method.Custom;
            };

            Adapter = adapter;
            bsAdapter.DataSource = adapter;

            // initialise the tree
            foreach (var n in Adapter.GetChildren())
                tree.Nodes.Add(Create(n));
        }

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
            Adapter.Add(e.Node.Tag as RetrieveFieldsViewAdapter.Node);
        }

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
                Adapter.Add(e.Data.GetData(typeof(RetrieveFieldsViewAdapter.SubjectNode)) as RetrieveFieldsViewAdapter.Node);
            else
                Adapter.Add(e.Data.GetData(typeof(RetrieveFieldsViewAdapter.FieldNode)) as RetrieveFieldsViewAdapter.Node);
        }

        #endregion

    }
}
