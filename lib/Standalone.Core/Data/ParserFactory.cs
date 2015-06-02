using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Builders;
using dbqf.Display.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Core.Data
{
    public class ParserFactory
    {
        /// <summary>
        /// Gets or sets a lookup to resolve fields to a parser for use with the UI.  If no lookup provided or field is not within lookup, default parsers will be used based on field data type.
        /// </summary>
        public Dictionary<dbqf.Configuration.IField, Parser> ParserLookup { get; set; }

        public Parser Create(FieldPath path, ParameterBuilder builder)
        {
            Parser parser = null;
            if (ParserLookup != null && ParserLookup.ContainsKey(path.Last))
                parser = ParserLookup[path.Last];

            if (path.Last.DataType == typeof(DateTime))
                parser = new ExtendedDateParser();
            else if (IsWholeNumber(path.Last.DataType))
                parser = new ConvertParser<object, long>();
            else if (IsDecimal(path.Last.DataType))
                parser = new ConvertParser<object, double>();

            return parser;
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
