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
            InitializeComponent();
            Adapter = adapter;
            this.DataContext = Adapter;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Adapter.Search(null);
        }
    }
}
