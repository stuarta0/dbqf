using System;
using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Criterion.Builders;
using dbqf.Display;

namespace dbqf.GtkSharp.UIElements
{
	public class GtkWidgetControlFactory : IControlFactory<Gtk.Widget>
    {
        /// <summary>
        /// Occurs when a control is generated that could support a list of suggestions.
        /// </summary>
        public event ListRequestedEventHandler ListRequested;
        private ListRequestedArgs OnListRequired(IFieldPath path)
        {
            var args = new ListRequestedArgs(path);
            if (ListRequested != null)
                ListRequested(this, args);
            return args;
        }

		public virtual UIElement<Gtk.Widget> Build(IFieldPath path, IParameterBuilder builder)
        {
			UIElement<Gtk.Widget> c;
            var f = path.Last;
            var listArgs = OnListRequired(path);
            if (builder is IJunctionBuilder)
                return Build(path, ((IJunctionBuilder)builder).Other);
//            else if (builder is IBetweenBuilder)
//            {
//                var between = new BetweenElement(Build(path, null), Build(path, null));
//                if (!String.IsNullOrEmpty(path.Last.DisplayFormat))
//                {
//                    // assume standard formatting codes
//                    // ^C\d*$ = currency (or "$#.00")
//                    // ^P\d*$ = percent (or "0.00\%")
//                    if (System.Text.RegularExpressions.Regex.IsMatch(path.Last.DisplayFormat.Trim(), @"^([Cc]\d*)|(\$.*)$"))
//                        between.PrefixText = System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol;
//                    else if (System.Text.RegularExpressions.Regex.IsMatch(path.Last.DisplayFormat.Trim(), @"^([Pp]\d*)|([^%]*%)$"))
//                        between.PostfixText = "%";
//                }
//                return between;
//            }
//            else if (builder is IBooleanBuilder || builder is INullBuilder)
//                return null;
//            else if (builder is INotBuilder)
//                return Build(path, ((INotBuilder)builder).Other);
//            else if (f.DataType == typeof(bool))
//                c = new CheckBoxElement();
//            else if (listArgs.List != null)
//            {
//                var style = ComboBoxStyle.DropDown;
//                if (listArgs.Type == FieldListType.Limited)
//                    style = ComboBoxStyle.DropDownList;
//                c = new ComboBoxElement(listArgs.List, style);
//            }
            else
				c = new EntryElement();
			
//            if (f.DataType == typeof(DateTime))
//                c = new DatePickerElement(c);

            return c;
        }
    }
}
