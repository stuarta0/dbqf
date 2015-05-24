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
            Element = _text;
        }

        public override object[] GetValues()
        {
            if (!String.IsNullOrEmpty(_text.Text))
            {
                if (Parser != null)
                    return Parser.Parse(_text.Text);
                return new object[] { _text.Text };
            }

            return null;
        }

        public override void SetValues(params object[] values)
        {
            if (Parser != null && values != null)
                values = Parser.Revert(values);

            if (values != null && values.Length >= 1 && values[0] != null)
                _text.Text = values[0].ToString();
        }
    }
}
