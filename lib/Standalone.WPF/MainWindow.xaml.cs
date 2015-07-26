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
            InitializeComponent();
            Adapter = adapter;
            this.DataContext = Adapter;

            //var adorner = new LoadingAdorner(ResultGrid);
            //var layer = AdornerLayer.GetAdornerLayer(ResultGrid);

            //Adapter.PropertyChanged += (s, e) =>
            //    {
            //        if ("IsSearching".Equals(e.PropertyName))
            //        {
            //            if (Adapter.IsSearching)
            //                layer.Add(adorner);
            //            else
            //                layer.Remove(adorner);
            //        }
            //    };
        }
    }
}
