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
        public ComboBoxElement(BindingList<object> list, ComboBoxStyle style)
        {
            var combo = new ComboBox();
            combo.DataSource = list;
            combo.DropDownStyle = style;

            combo.TextChanged += (sender, e) => OnChanged();
            combo.SelectedIndexChanged += (sender, e) => OnChanged();
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
                    combo.SelectedItem = values[0];
                else
                    combo.Text = values[0].ToString();
            }
        }
    }
}
