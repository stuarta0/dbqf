using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Standalone.WPF.Controls;

namespace Standalone.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowAdapter Adapter { get; private set; }
        public MainWindow(MainWindowAdapter adapter)
        {
            //InitializeComponent();
            Adapter = adapter;
            this.DataContext = Adapter;
        }

        private void ResultGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column is DataGridBoundColumn)
            {
                var column = (DataGridBoundColumn)e.Column;
                
                var data = ((System.Windows.Controls.DataGrid)sender).ItemsSource as System.Data.DataView;
                if (data != null)
                {
                    if (data.Table.Columns.Contains(e.PropertyName))
                    {
                        var path = (dbqf.Criterion.IFieldPath)data.Table.Columns[e.PropertyName].ExtendedProperties["FieldPath"];
                        column.Binding.StringFormat = path.Last.DisplayFormat;
                    }
                }
            }
        }
    }
}
