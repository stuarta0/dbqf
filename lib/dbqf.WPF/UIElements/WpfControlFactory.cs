using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace dbqf.WPF.UIElements
{
    public class WpfControlFactory: IControlFactory<UIElement>
    {
        public event ListRequestedEventHandler ListRequested;
        private ListRequestedArgs OnListRequired(FieldPath path)
        {
            var args = new ListRequestedArgs(path);
            if (ListRequested != null)
                ListRequested(this, args);
            return args;
        }

        public UIElement<UIElement> Build(FieldPath path, ParameterBuilder builder)
        {
            UIElement<System.Windows.UIElement> c;
            var f = path.Last;
            var listArgs = OnListRequired(path);
            if (builder is BetweenBuilder || builder is DateBetweenBuilder)
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
                var isEditable = true;
                if (listArgs.Type == Configuration.FieldListType.Limited)
                    isEditable = false;
                c = new ComboBoxElement(listArgs.List, isEditable);
            }
            else
                c = new TextBoxElement();

            return c;
        }
    }
}
