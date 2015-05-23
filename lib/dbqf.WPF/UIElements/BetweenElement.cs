using dbqf.Display;
using dbqf.WPF.UIElements.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.WPF.UIElements
{
    public class BetweenElement : UIElement<System.Windows.UIElement>
    {
        /// <summary>
        /// Gets or sets the label that appears before any other controls.
        /// </summary>
        public string PrefixText
        {
            get { return _between.PrefixText; }
            set { _between.PrefixText = value; }
        }

        /// <summary>
        /// Gets or sets the label that appears after all other controls.
        /// </summary>
        public string PostfixText
        {
            get { return _between.PostfixText; }
            set { _between.PostfixText = value; }
        }

        private BetweenControl _between; // same instance as base.Element
        private UIElement<System.Windows.UIElement> _element1, _element2;
        public BetweenElement(UIElement<System.Windows.UIElement> element1, UIElement<System.Windows.UIElement> element2)
        {
            _element1 = element1;
            _element2 = element2;

            // it's expected that the controls themselves in the child UIElements won't change
            _between = new BetweenControl();
            _between.Control1 = element1.Element;
            _between.Control2 = element2.Element;

            element1.Changed += Element_Changed;
            element1.Search += Element_Search;
            element2.Changed += Element_Changed;
            element2.Search += Element_Search;

            Element = _between;
        }

        public override void Dispose()
        {
            if (_element1 != null)
                _element1.Dispose();
            if (_element2 != null)
                _element2.Dispose();
        }

        private void Element_Search(object sender, EventArgs e)
        {
            OnSearch();
        }

        private void Element_Changed(object sender, EventArgs e)
        {
            OnChanged();
        }

        /// <summary>
        /// Returns a BetweenValue of the two child controls values.
        /// </summary>
        /// <returns></returns>
        public override object[] GetValues()
        {
            var values1 = GetValue(_element1.GetValues());
            var values2 = GetValue(_element2.GetValues());

            if (values1 != null || values2 != null)
            {
                var result = new object[] { new BetweenValue(values1, values2) };
                if (Parser != null)
                    return Parser.Parse(result);
                return result;
            }

            return null;
        }

        public override void SetValues(params object[] values)
        {
            // so we're expecting BetweenValues when we enter here
            // any other values and we'll set element1's values to values[0], and element2 to values[1]
            if (Parser != null)
                values = Parser.Revert(values);

            if (values != null && values.Length >= 1)
            {
                if (values[0] is BetweenValue)
                {
                    var between = (BetweenValue)values[0];
                    _element1.SetValues(between.From);
                    _element2.SetValues(between.To);
                }
                else
                {
                    _element1.SetValues(values[0]);
                    if (values.Length >= 2)
                        _element2.SetValues(values[1]);
                }
            }
        }

        protected virtual object GetValue(object[] values)
        {
            if (values != null && values.Length == 1)
                return values[0];
            return values;
        }
    }
}
