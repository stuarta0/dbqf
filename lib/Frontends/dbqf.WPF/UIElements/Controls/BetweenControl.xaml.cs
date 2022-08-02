using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace dbqf.WPF.UIElements.Controls
{
    /// <summary>
    /// Interaction logic for BetweenControl.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class BetweenControl : UserControl
    {
        public BetweenControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the label that appears before any other controls.
        /// </summary>
        public string PrefixText { get; set; }
        public Visibility PrefixVisibility
        {
            get { return String.IsNullOrEmpty(PrefixText) ? Visibility.Collapsed : Visibility.Visible; }
        }

        /// <summary>
        /// Gets or sets the label that appears after all other controls.
        /// </summary>
        public string PostfixText { get; set; }
        public Visibility PostfixVisibility
        {
            get { return String.IsNullOrEmpty(PostfixText) ? Visibility.Collapsed : Visibility.Visible; }
        }

        /// <summary>
        /// Sets the first or 'from' control.
        /// </summary>
        public UIElement Control1 { get; set; }

        /// <summary>
        /// Sets the second or 'to' control.
        /// </summary>
        public UIElement Control2 { get; set; }
    }
}
