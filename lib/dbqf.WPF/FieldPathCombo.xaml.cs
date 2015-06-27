using dbqf.Display;
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
        public FieldPathCombo()
        {
            InitializeComponent();
        }
        public FieldPathCombo(FieldPathComboAdapter adapter)
            : this()
        {
            SetAdapter(this, adapter);
        }

        /// <summary>
        /// Gets the value of the <c>FieldPathComboAdapter</c> attached property for the specified object.
        /// </summary>
        /// <param name="obj">The element from which to read the property value.</param>
        /// <returns>The adapter of the combo.</returns>
        public static FieldPathComboAdapter GetAdapter(FieldPathCombo obj)
        {
            return (FieldPathComboAdapter)obj.GetValue(AdapterProperty);
        }

        /// <summary>
        /// Sets the value of the <c>FieldPathComboAdapter</c> attached property for the specified object.
        /// </summary>
        /// <param name="obj">The element from which to set the property value.</param>
        /// <param name="value">The adapter of the combo.</param>
        public static void SetAdapter(FieldPathCombo obj, FieldPathComboAdapter value)
        {
            obj.SetValue(AdapterProperty, value);
            obj.DataContext = value;
        }

        /// <summary>
        /// Identifies the <c>FieldPathComboAdapter</c> attached property.
        /// </summary>
        public static readonly DependencyProperty AdapterProperty =
            DependencyProperty.RegisterAttached(
              "Adapter",
              typeof(FieldPathComboAdapter),
              typeof(FieldPathCombo),
              new UIPropertyMetadata(null));
    }
}
