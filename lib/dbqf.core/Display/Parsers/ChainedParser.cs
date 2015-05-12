using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Display.Parsers
{
    /// <summary>
    /// Takes input and runs through each parser in order.
    /// </summary>
    public class ChainedParser : Parser
    {
        public IList<Parser> Parsers { get; protected set; }

        public ChainedParser()
        {
            Parsers = new List<Parser>();
        }

        /// <summary>
        /// Fluently add parsers to the chained parser.
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public ChainedParser Add(Parser parser)
        {
            Parsers.Add(parser);
            return this;
        }

        /// <summary>
        /// Run input through each parser, passing the result of the previous parser to the next and returning the result after the final parser has executed.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public override object[] Parse(params object[] values)
        {
            foreach (var p in Parsers)
                values = p.Parse(values);
            return values;
        }

        /// <summary>
        /// Chained parser reverts in reverse order.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public override object[] Revert(params object[] values)
        {
            for (int i = Parsers.Count - 1; i >= 0; i--)
                values = Parsers[i].Revert(values);
            return values;
        }
    }
}
