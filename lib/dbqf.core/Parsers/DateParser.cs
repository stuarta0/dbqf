using System;
using System.Collections.Generic;
using dbqf.Criterion.Values;

namespace dbqf.Parsers
{
    /// <summary>
    /// Parses free text such as 'Mar 1985' into bounding values '1985-03-01' and '1985-04-01'.
    /// </summary>
    public class DateParser : Parser
    {
        /// <summary>
        /// Gets or sets a value indicating whether parsing an invalid date will return null.
        /// </summary>
        public bool AllowNulls
        {
            get { return _allowNulls; }
            set 
            { 
                _allowNulls = value;
                ComputeHash();
            }
        }
        private bool _allowNulls;

        /// <summary>
        /// Given each value in the parameters, determine the date boundaries.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public override object[] Parse(params object[] values)
        {
            var result = new List<object>();
            foreach (var v in values)
            {
                var date = ParseDate(v);
                if (date != null || AllowNulls)
                    result.Add(date);
            }
            return result.ToArray();
        }

        /// <summary>
        /// Reverts any number of DateValue values back to string representations.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public override object[] Revert(params object[] values)
        {
            if (values == null)
                return null;

            var result = new List<object>();
            foreach (var v in values)
            {
                var d = (DateValue)v;
                var t = d.Upper - d.Lower;
                if (t.Days == 1)
                    result.Add(d.Lower.ToShortDateString());
                else if (t.Days >= 365)
                    result.Add(d.Lower.ToString("yyyy"));
                else
                    result.Add(d.Lower.ToString("MMM yyyy"));
            }

            return result.ToArray();
        }

        protected virtual object ParseDate(object value)
        {
            if (value is DateTime)
            {
                return new DateValue(((DateTime)value).Date, ((DateTime)value).Date.AddDays(1));
            }
            else if (value is BetweenValue)
            {
                var between = value as BetweenValue;
                between.From = between.From == null ? null : ParseDate(between.From);
                between.To = between.To == null ? null : ParseDate(between.To);
                return between;
            }
            else if (value is string)
            {
                var date = (string)value;

                var ci = new System.Globalization.CultureInfo("en-AU");
                var formats = GetDateTimeFormats();
                DateTime result = DateTime.MinValue;

                for (int i = 0; i < formats.Length; i++)
                {
                    var format = formats[i];
                    if (DateTime.TryParseExact(date, format, ci, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out result))
                    {
                        var end = result.Date;
                        if (format.Contains("d"))
                            end = end.AddDays(1);
                        else if (format.Contains("M"))
                            end = end.AddMonths(1); // we have our month, but not our day
                        else if (format.Contains("y"))
                            end = end.AddYears(1);  // we have our year, but not our month/day

                        return new DateValue(result.Date, end);
                    }
                }

                // if we can parse a date using the built-in then we're dealing with a single day
                if (DateTime.TryParse(date, ci, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out result))
                    return new DateValue(result.Date, result.Date.AddDays(1));
            }

            // unable to parse
            throw new FormatException(String.Format("Could not intepret date value '{0}'.", value));
        }

        private static string[] _formats = null;
        private string[] GetDateTimeFormats()
        {
            // combine a whole bunch of different combinations of year, month and day to capture anything the user types in
            if (_formats == null)
            {
                string[] years = new string[] { "yyyy", "yy" };
                string[] months = new string[] { "M", "MMM", "MMMM" };
                string[] days = new string[] { "d", "ddd", "dddd" };

                List<string> formats = new List<string>();
                for (int y = 0; y < years.Length; y++)
                {
                    formats.Add(years[y]);
                    for (int m = 0; m < months.Length; m++)
                    {
                        formats.Add(String.Concat(months[m], "/", years[y]));
                        formats.Add(String.Concat(months[m], ",", years[y]));
                        formats.Add(String.Concat(months[m], " ", years[y]));
                        for (int d = 0; d < days.Length; d++)
                        {
                            formats.Add(String.Concat(days[d], "/", months[m], "/", years[y]));
                            formats.Add(String.Concat(days[d], " ", months[m], ",", years[y]));
                            formats.Add(String.Concat(days[d], " ", months[m], " ", years[y]));
                        }
                    }
                }

                // more specific formats (that are not already determined via built-in DateTime.TryParse)
                formats.Add("yyyy/M");
                formats.Add("yyyyMMdd");
                formats.Add("yyyy-MM-ddThh:mm:ss");
                _formats = formats.ToArray();
            }

            return _formats;
        }

        public override bool Equals(object obj)
        {
            if (obj is DateParser)
                return AllowNulls.Equals(((DateParser)obj).AllowNulls);
            return base.Equals(obj);
        }
        protected override void ComputeHash()
        {
            base.ComputeHash();
            _hash = (_hash * 7) + AllowNulls.GetHashCode();
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
