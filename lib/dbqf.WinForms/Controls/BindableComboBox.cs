using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace dbqf.WinForms.Controls
{
    // http://stackoverflow.com/questions/8392019/control-not-immediately-updating-bound-property-with-inotifypropertychanged
    /// <summary>
    /// A modification of the standard <see cref="ComboBox"/> in which a data binding
    /// on the SelectedItem property with the update mode set to DataSourceUpdateMode.OnPropertyChanged
    /// actually updates when a selection is made in the combobox.
    /// </summary>
    public class BindableComboBox : ComboBox
    {
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ComboBox.SelectionChangeCommitted"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnSelectionChangeCommitted(EventArgs e)
        {
            base.OnSelectionChangeCommitted(e);

            foreach (Binding x in this.DataBindings)
            {
                if (x.PropertyName == "SelectedItem" && x.DataSourceUpdateMode == DataSourceUpdateMode.OnPropertyChanged)
                {
                    // Force the binding to update from the new SelectedItem
                    x.WriteValue();

                    // Force the Textbox to update from the binding
                    x.ReadValue();
                }
            }
        }
    }
}
