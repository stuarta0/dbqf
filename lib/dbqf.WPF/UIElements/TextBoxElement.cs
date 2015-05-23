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
            throw new NotImplementedException();
        }

        public override void SetValues(params object[] values)
        {
            throw new NotImplementedException();
        }
    }
}
