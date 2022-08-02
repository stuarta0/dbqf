﻿using System;
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
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class LoadingControl : UserControl
    {
        public LoadingControl()
        {
            //InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindowAdapter)this.DataContext).CancelSearch();
        }
    }
}
