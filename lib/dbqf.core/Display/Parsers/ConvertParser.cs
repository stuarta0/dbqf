using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Display.Parsers
{
    /// <summary>
    /// Parses objects using built-in Convert.ToType() using the class property ConvertTo.
    /// </summary>
    public class ConvertParser<Tfrom, Tto> : Parser
    {
        public ConvertParser()
        {
        }

        /// <summary>
        /// Uses the built-in Convert class to try and convert the values.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public override object[] Parse(params object[] values)
        {
            return Parse(typeof(Tto), values);
        }

        /// <summary>
        /// Uses the built-in Convert class to try and convert the values.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public override object[] Revert(params object[] values)
        {
            return Parse(typeof(Tfrom), values);
        }

        protected virtual object[] Parse(Type to, params object[] values)
        {
            if (values == null)
                return null;

            var result = new List<object>();
            foreach (var v in values)
            {
                if (v != null)
                {
                    try { result.Add(Convert.ChangeType(v, to)); }
                    catch (Exception ex)
                    {
                        throw new FormatException(String.Format("Could not convert value '{0}' to {1}.", v, to.Name), ex);
                    }
                }
            }
            return result.ToArray();
        }
    }
}
