using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Parsers
{
    public abstract class Parser
    {
        /// <summary>
        /// Parse one or more values and return the new set of parsed values.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public abstract object[] Parse(params object[] values);

        /// <summary>
        /// Given a set of already-parsed values, attempt to reverse the parse operation.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public abstract object[] Revert(params object[] values);
    }
}
