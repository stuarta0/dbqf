using dbqf.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace dbqf.WPF.UIElements
{
    public class TextBoxElement : UIElement<System.Windows.UIElement>
    {
        private TextBox _text;
        public TextBoxElement()
        {
            _text = new TextBox();
            _text.VerticalContentAlignment = VerticalAlignment.Center;
            _text.TextInput += delegate { OnChanged(); };
            _text.KeyDown += Text_KeyDown;
            Element = _text;
        }

        void Text_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                OnSearch();
        }

        public override object[] GetValues()
        {
            if (!String.IsNullOrEmpty(_text.Text))
                return new object[] { _text.Text };
            return null;
        }

        public override void SetValues(params object[] values)
        {
            if (values != null && values.Length > 0 && values[0] != null)
                _text.Text = values[0].ToString();
            else
                _text.Text = null;
        }
    }
}
