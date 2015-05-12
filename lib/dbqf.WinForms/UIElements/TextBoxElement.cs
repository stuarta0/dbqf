using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Preset;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace dbqf.WinForms.UIElements
{
    public class TextBoxElement : ErrorProviderElement
    {
        private TextBox _text;
        public virtual TextBox TextBox
        {
            get { return _text; }
            set 
            {
                if (_text != null)
                {
                    _text.TextChanged -= OnTextChanged;
                    _text.KeyDown -= OnKeyDown;
                }

                _text = value;
                Element = _text;
                if (_text != null)
                {
                    _text.TextChanged += OnTextChanged;
                    _text.KeyDown += OnKeyDown;
                }
            }
        }

        void OnTextChanged(object sender, EventArgs e)
        {
            OnChanged();
        }

        void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                OnSearch();
        }

        public TextBoxElement()
        {
            TextBox = new TextBox();
        }

        public override object[] GetValues()
        {
            if (!String.IsNullOrEmpty(Element.Text))
            {
                if (Parser != null)
                    return Parser.Parse(Element.Text);
                return new object[] { Element.Text };
            }

            return null;
        }

        public override void SetValues(params object[] values)
        {
            if (Parser != null && values != null)
                values = Parser.Revert(values);

            if (values != null && values.Length >= 1 && values[0] != null)
                Element.Text = values[0].ToString();
        }
    }
}
