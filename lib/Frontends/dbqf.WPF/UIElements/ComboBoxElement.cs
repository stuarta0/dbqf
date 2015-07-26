using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Preset;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Markup;
using System.IO;
using System.Windows;

namespace dbqf.WPF.UIElements
{
    public class ComboBoxElement : UIElement<System.Windows.UIElement>
    {
        public ComboBoxElement(BindingList<object> list, bool isEditable)
        {
            // use XAML to construct our combo with virtualizing stack panel to alleviate massive lists
            var xaml = @"
    <ComboBox VirtualizingStackPanel.IsVirtualizing=""True"" VirtualizingStackPanel.VirtualizationMode=""Recycling"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
        <ComboBox.ItemsPanel>
            <ItemsPanelTemplate >
                <VirtualizingStackPanel/>
            </ItemsPanelTemplate>
        </ComboBox.ItemsPanel>
    </ComboBox>";

            var combo = (ComboBox)XamlReader.Load(System.Xml.XmlReader.Create(new StringReader(xaml)));
            combo.ItemsSource = list;
            combo.IsEditable = isEditable;
            combo.IsTextSearchEnabled = true;

            combo.TextInput += (sender, e) => OnChanged();
            combo.SelectionChanged += (sender, e) => OnChanged();
            combo.KeyDown += (sender, e) => { if (e.Key == System.Windows.Input.Key.Enter) OnSearch(); };
            Element = combo;
        }

        public override object[] GetValues()
        {
            var combo = (ComboBox)Element;
            if (!combo.IsEditable && combo.SelectedItem != null && !String.IsNullOrEmpty(combo.SelectedItem.ToString()))
                return new object[] { combo.SelectedItem };
            else if (!String.IsNullOrEmpty(combo.Text))
                return new object[] { combo.Text };
            return null;
        }

        public override void SetValues(params object[] values)
        {
            var combo = (ComboBox)Element;
            if (values != null && values.Length > 0 && values[0] != null)
            {
                if (!combo.IsEditable)
                    combo.SelectedItem = values[0];
                else
                    combo.Text = values[0].ToString();
            }
            else
            {
                if (!combo.IsEditable)
                    combo.SelectedItem = null;
                else
                    combo.Text = null;
            }
        }
    }
}
