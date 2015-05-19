using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using dbqf.Display;
using dbqf.WinForms.UIElements.Controls;

namespace dbqf.WinForms.UIElements
{
    public class DatePickerElement : UIElement<Control>
    {
        private DatePickerControl _control;
        public DatePickerElement(UIElement<Control> other)
        {
            _control = new DatePickerControl();
            other.Element.Margin = Padding.Empty;
            _control.Other = other;
            
            other.Changed += (s, e) => OnChanged();
            other.Search += (s, e) => OnSearch();

            Element = _control;
        }

        public override object[] GetValues()
        {
            return _control.Other.GetValues();
        }

        public override void SetValues(params object[] values)
        {
            _control.Other.SetValues(values);
        }
    }
}
