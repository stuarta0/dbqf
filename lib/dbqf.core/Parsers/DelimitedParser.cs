using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Parsers
{
    /// <summary>
    /// Parses free text such as 'Mar 1985' into bounding values '1985-03-01' and '1985-04-01'.
    /// </summary>
    public class DelimitedParser : Parser
    {
        public string[] Delimiters { get; set; }

        public DelimitedParser()
        {
        }

        public DelimitedParser(params string[] delimiters)
        {
            Delimiters = delimiters;
        }

        /// <summary>
        /// Tries to split each value given into an array of each item based on the parameter (calls ToString on value). e.g.
        /// object[2] { "123,456, 789", "abc " } gives
        /// object[4] { "123", "456", "789", "abc" }
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public override object[] Parse(params object[] values)
        {
            var result = new List<object>();
            foreach (var v in values)
            {
                if (v != null)
                {
                    var str = v.ToString();
                    var split = str.Split(Delimiters, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < split.Length; i++)
                        result.Add(split[i].Trim());
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// Using the first delimiter, join all the given values together.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public override object[] Revert(params object[] values)
        {
            if (values == null || Delimiters.Length == 0)
                return null;

            string[] strs = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
                strs[i] = values[i].ToString();

            return new object[] { String.Join(Delimiters[0], strs) };
        }
    }
}
