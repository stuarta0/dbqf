using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Builders;
using dbqf.Display.Parsers;
using dbqf.Display.Preset;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace dbqf.WinForms.UIElements
{
    public class WinFormsControlFactory : IControlFactory<Control>
    {
        /// <summary>
        /// Occurs when a control is generated that could support a list of suggestions.
        /// </summary>
        public event ListRequestedEventHandler ListRequested;
        private ListRequestedArgs OnListRequired(FieldPath path)
        {
            var args = new ListRequestedArgs(path);
            if (ListRequested != null)
                ListRequested(this, args);
            return args;
        }

        public virtual UIElement<Control> Build(FieldPath path, ParameterBuilder builder)
        {
            UIElement<Control> c;
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
                var style = ComboBoxStyle.DropDown;
                if (listArgs.Type == FieldListType.Limited)
                    style = ComboBoxStyle.DropDownList;
                c = new ComboBoxElement(listArgs.List, style);
            }
            else
                c = new TextBoxElement();

            if (f.DataType == typeof(DateTime))
            {
                c.Parser = new DateParser();
                c = new DatePickerElement(c);
            }
            else if (builder is LikeBuilder)
                c.Parser = new ConvertParser<object, string>();

            return c;
        }
    }
}
