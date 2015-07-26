using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Preset;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;

namespace dbqf.WPF.UIElements
{
    public class ListBoxElement : UIElement<System.Windows.UIElement>
    {
        public ListBoxElement(BindingList<object> data, double? maxHeight = null)
        {
            var list = new ListBox();
            list.ItemsSource = data;
            list.SelectionMode = SelectionMode.Extended;
            if (maxHeight.HasValue)
                list.MaxHeight = maxHeight.Value;

            list.SelectionChanged += (sender, e) => OnChanged();
            list.KeyDown += (sender, e) => { if (e.Key == System.Windows.Input.Key.Enter) OnSearch(); };
            Element = list;
        }

        public override object[] GetValues()
        {
            var list = (ListBox)Element;
            if (list.SelectedItems.Count == 0)
                return null;

            object[] values = new object[list.SelectedItems.Count];
            for (int i = 0; i < list.SelectedItems.Count; i++)
                values[i] = list.SelectedItems[i];

            return values;
        }

        public override void SetValues(params object[] values)
        {
            var list = (ListBox)Element;
            if (values != null && values.Length > 0)
            {
                foreach (var item in values)
                    list.SelectedItems.Add(item);
            }
            else
                list.SelectedItems.Clear();
        }
    }
}
