using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Standalone.WPF.Controls
{
    /// <summary>
    /// Interaction logic for RetrieveFieldsView.xaml
    /// </summary>
    public partial class RetrieveFieldsView : UserControl
    {
        public RetrieveFieldsViewAdapter Adapter { get; private set; }
        public RetrieveFieldsView(RetrieveFieldsViewAdapter adapter)
        {
            //InitializeComponent();
            Adapter = adapter;
            this.DataContext = adapter;
        }

        private void Add(RetrieveFieldsViewAdapter.Node node)
        {
            var added = Adapter.Add(node);
            var lstCustom = FindName("lstCustom") as ListBox;
            lstCustom.SelectedItems.Clear();
            foreach (var f in added)
                lstCustom.SelectedItems.Add(f);
        }

        private void TreeView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var tree = sender as TreeView;
                if (tree.SelectedItem != null)
                    DragDrop.DoDragDrop(tree, tree.SelectedItem, DragDropEffects.Move);
            }
        }

        private void ListBox_DragEnter(object sender, DragEventArgs e)
        {
            // apparently GetData doesn't understand inheritence
            if (e.Data.GetDataPresent(typeof(RetrieveFieldsViewAdapter.SubjectNode))
                || e.Data.GetDataPresent(typeof(RetrieveFieldsViewAdapter.FieldNode)))
                e.Effects = DragDropEffects.Move;
            else
                e.Effects = DragDropEffects.None;
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(RetrieveFieldsViewAdapter.SubjectNode)))
                Add(e.Data.GetData(typeof(RetrieveFieldsViewAdapter.SubjectNode)) as RetrieveFieldsViewAdapter.Node);
            else
                Add(e.Data.GetData(typeof(RetrieveFieldsViewAdapter.FieldNode)) as RetrieveFieldsViewAdapter.Node);
        }

        private void ListBox_KeyDown(object sender, KeyEventArgs e)
        {
            var lstCustom = e.Source as ListBox;

            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                foreach (var f in new List<IFieldPath>(lstCustom.SelectedItems.Cast<IFieldPath>()))
                    Adapter.Fields.Remove(f);
            }
        }

        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var text = e.OriginalSource as TextBlock;
            if (text != null && text.DataContext is RetrieveFieldsViewAdapter.Node)
                Add((RetrieveFieldsViewAdapter.Node)text.DataContext);
        }
    }
}
