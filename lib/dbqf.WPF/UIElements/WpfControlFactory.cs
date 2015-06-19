using System;
using System.Windows;
using dbqf.Criterion;
using dbqf.Criterion.Builders;
using dbqf.Display;

namespace dbqf.WPF.UIElements
{
    public class WpfControlFactory: IControlFactory<UIElement>
    {
        public event ListRequestedEventHandler ListRequested;
        private ListRequestedArgs OnListRequired(IFieldPath path)
        {
            var args = new ListRequestedArgs(path);
            if (ListRequested != null)
                ListRequested(this, args);
            return args;
        }

        public UIElement<UIElement> Build(IFieldPath path, ParameterBuilder builder)
        {
            UIElement<System.Windows.UIElement> c;
            var f = path.Last;
            var listArgs = OnListRequired(path);
            if (builder is JunctionBuilder)
                return Build(path, ((JunctionBuilder)builder).Other);
            else if (builder is BetweenBuilder || builder is DateBetweenBuilder)
            {
                var between = new BetweenElement(Build(path, null), Build(path, null));
                if (!String.IsNullOrEmpty(path.Last.DisplayFormat))
                {
                    // assume standard formatting codes
                    // ^C\d*$ = currency (or "$#.00")
                    // ^P\d*$ = percent (or "0.00\%")
                    if (System.Text.RegularExpressions.Regex.IsMatch(path.Last.DisplayFormat.Trim(), @"^([Cc]\d*)|(\$.*)$"))
                        between.PrefixText = System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol;
                    else if (System.Text.RegularExpressions.Regex.IsMatch(path.Last.DisplayFormat.Trim(), @"^([Pp]\d*)|([^%]*%)$"))
                        between.PostfixText = "%";
                }
                return between;
            }
            else if (builder is BooleanBuilder || builder is NullBuilder)
                return null;
            else if (builder is NotBuilder)
                return Build(path, ((NotBuilder)builder).Other);
            else if (f.DataType == typeof(bool))
                c = new CheckBoxElement();
            else if (listArgs.List != null)
            {
                if (listArgs.Type == Configuration.FieldListType.Limited)
                    c = new ListBoxElement(listArgs.List, 400);
                else
                    c = new ComboBoxElement(listArgs.List, true);
            }
            else
                c = new TextBoxElement();

            return c;
        }
    }
}
