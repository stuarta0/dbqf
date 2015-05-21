using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Builders;
using dbqf.Display.Parsers;
using dbqf.WinForms.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Standalone.Forms;
using Standalone.Core.Data;

namespace Standalone.Forms
{
    public class ExtendedControlFactory : WinFormsControlFactory
    {
        /// <summary>
        /// Gets or sets a lookup to resolve fields to a parser for use with the UI.  If no lookup provided or field is not within lookup, default parsers will be used based on field data type.
        /// </summary>
        public Dictionary<dbqf.Configuration.IField, Parser> ParserLookup { get; set; }

        public override UIElement<Control> Build(FieldPath path, ParameterBuilder builder)
        {
            var c = base.Build(path, builder);
            if (c == null)
                return null;

            var parser = c.Parser;
            if (ParserLookup != null && ParserLookup.ContainsKey(path.Last))
                parser = ParserLookup[path.Last];

            // use the internal builder if its a NotBuilder
            if (builder is NotBuilder)
                builder = ((NotBuilder)builder).Other;

            if (!(builder is DateBetweenBuilder || builder is BetweenBuilder))
            {
                if (path.Last.DataType == typeof(DateTime))
                    parser = new ExtendedDateParser();
                else if (IsWholeNumber(path.Last.DataType))
                    parser = new ConvertParser<object, long>();
                else if (IsDecimal(path.Last.DataType))
                    parser = new ConvertParser<object, double>();
            }

            // hook either the base parser (WinFormsControlFactory), custom parser (via ParserLookup), or default parser (determined in this method block)
            c.Parser = parser;

            // custom behaviour: if our control has a delimited parser and it's a textbox, replace the implementation and convert Ctrl+V with newlines into delimited values
            if (c is TextBoxElement)
            {
                var delimiter = ParserContains<DelimitedParser>(c.Parser);
                if (delimiter != null && delimiter.Delimiters.Length > 0)
                {
                    var text = new PasteOverrideTextBox();
                    ((TextBoxElement)c).TextBox = text;
                    text.Pasted += (sender, e) =>
                    {
                        e.Text = e.Text.Replace(Environment.NewLine, delimiter.Delimiters[0]);
                    };
                }
            }

            if (c is ErrorProviderElement)
                ((ErrorProviderElement)c).ShowError = true;

            return c;
        }

        private T ParserContains<T>(Parser instance)
            where T : Parser
        {
            if (instance is ChainedParser)
            {
                var queue = new Queue<Parser>(((ChainedParser)instance).Parsers);
                while (queue.Count > 0)
                {
                    var item = queue.Dequeue();
                    if (item is T)
                        return (T)item;

                    if (item is ChainedParser)
                    {
                        // go further down the rabbit hole
                        foreach (var p in ((ChainedParser)item).Parsers)
                            queue.Enqueue(p);
                    }
                }
            }

            return null;
        }

        private bool IsWholeNumber(Type t)
        {
            foreach (var numericType in new Type[] { typeof(sbyte), typeof(short), typeof(int), typeof(long), typeof(byte), typeof(ushort), typeof(uint), typeof(ulong) })
                if (t == numericType)
                    return true;

            return false;
        }

        private bool IsDecimal(Type t)
        {
            foreach (var numericType in new Type[] { typeof(Single), typeof(double), typeof(decimal) })
                if (t == numericType)
                    return true;

            return false;
        }
    }
}
