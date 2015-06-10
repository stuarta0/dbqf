using System;
using System.Collections.Generic;
using dbqf.Criterion.Values;

namespace dbqf.Parsers
{
    /// <summary>
    /// Parses delimited strings into individual values.
    /// </summary>
    /// <example>
    /// new DelimitedParser(",").Parse("123,abc"); // gives object[] { "123", "abc" }
    /// </example>
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
                    if (v is BetweenValue)
                    {
                        var between = v as BetweenValue;
                        between.From = Parse(between.From);
                        between.To = Parse(between.To);
                        result.Add(between);
                    }
                    else
                    {
                        var str = v.ToString();
                        var split = str.Split(Delimiters, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < split.Length; i++)
                            result.Add(split[i].Trim());
                    }
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

        public override bool Equals(object obj)
        {
            if (obj is DelimitedParser)
            {
                var other = obj as DelimitedParser;
                if (Delimiters.Length == other.Delimiters.Length)
                {
                    for (int i = 0; i < Delimiters.Length; i++)
                        if (!Delimiters[i].Equals(other.Delimiters[i]))
                            return false;
                    return true;
                }
                return false;
            }

            return base.Equals(obj);
        }
    }
}
