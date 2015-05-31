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
    /// Interaction logic for FieldPathCombo.xaml
    /// </summary>
    public partial class FieldPathCombo : UserControl
    {
        public FieldPathComboAdapter Adapter { get; private set; }
        public FieldPathCombo(FieldPathComboAdapter adapter)
        {
            InitializeComponent();
            Adapter = adapter;
            this.DataContext = Adapter;
        }
    }
}
