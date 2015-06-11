using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Preset;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace dbqf.WinForms.UIElements
{
    public class ComboBoxElement : ErrorProviderElement
    {
        /// <summary>
        /// Cached value is used when DropDownList SelectedItem is set before ComboBox.DataSource, or BindingList.ResetBindings() is called.
        /// </summary>
        private object _cache = null;
        public ComboBoxElement(BindingList<object> list, ComboBoxStyle style)
        {
            var combo = new ComboBox();
            combo.DataSource = list;
            combo.DropDownStyle = style;

            // BindingList<T>.ResetBindings() when set as ComboBox.DataSource will clear the SelectedValue (and subsequently Text) of the combo
            // therefore, hook ListChanged, cache the current SelectedValue/Text and on SelectedValueChanged, check if there's a cached value and set it.
            // Also, if SetValues() is called before the list data exists and we have DropDownList, we'll need to cache the value for when the list updates.
            list.ListChanged += (s, e) => 
            {
                if (_cache == null)
                    _cache = style == ComboBoxStyle.DropDownList ? combo.SelectedItem : combo.Text;
            };
            combo.SelectedValueChanged += (s, e) => 
            { 
                if (_cache != null)
                {
                    var value = _cache;
                    _cache = null;
                    if (style == ComboBoxStyle.DropDownList)
                        combo.SelectedItem = value;
                    else
                        combo.Text = value.ToString();
                }
            };

            // TextChanged handles list selection and user input
            combo.TextChanged += (sender, e) =>{ OnChanged(); };

            // SelectedIndexChanged handles DropDownList selection
            combo.SelectedIndexChanged += (sender, e) => { OnChanged(); };

            // Detect Enter key and trigger search
            combo.KeyDown += (sender, e) => { if (e.KeyCode == Keys.Enter) OnSearch(); };
            Element = combo;

            dbqf.WinForms.Extensions.ComboBoxExtension.AdjustWidthOnDropDown(combo);
        }

        public override object[] GetValues()
        {
            var combo = (ComboBox)Element;
            object[] values = null;
            if (combo.DropDownStyle == ComboBoxStyle.DropDownList && combo.SelectedItem != null && !String.IsNullOrEmpty(combo.SelectedItem.ToString()))
                values = new object[] { combo.SelectedItem };
            else if (!String.IsNullOrEmpty(combo.Text))
                values = new object[] { combo.Text };
            return values;
        }

        public override void SetValues(params object[] values)
        {
            var combo = (ComboBox)Element;
            if (values == null || values.Length == 0 || values[0] == null)
            {
                if (combo.DropDownStyle == ComboBoxStyle.DropDownList)
                    combo.SelectedItem = null;
                else
                    combo.Text = null;
            }
            else
            {
                if (combo.DropDownStyle == ComboBoxStyle.DropDownList)
                    combo.SelectedItem = (_cache = values[0]);
                else
                    combo.Text = (_cache = values[0]).ToString();
            }
        }
    }
}
