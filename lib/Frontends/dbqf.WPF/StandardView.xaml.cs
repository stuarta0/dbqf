using dbqf.Display.Standard;
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

namespace dbqf.WPF
{
    /// <summary>
    /// Interaction logic for StandardView.xaml
    /// </summary>
    public partial class StandardView : UserControl
    {
        public StandardAdapter<UIElement> Adapter { get; private set; }
        public StandardView(StandardAdapter<UIElement> adapter)
        {
            InitializeComponent();
            Adapter = adapter;
            this.DataContext = Adapter;
        }
    }
}
