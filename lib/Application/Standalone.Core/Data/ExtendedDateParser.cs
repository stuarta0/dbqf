using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using dbqf.Criterion.Values;
using dbqf.Parsers;

namespace Standalone.Core.Data
{
    public class ExtendedDateParser : DateParser
    {
        //hello
        //today
        // sometime ago
        // yesterday 
        //last week
        //last 3 weeks
        //2 days ago
        //14 days ago
        //last month
        //last year
        //3 months ago
        //last FY
        //next month
        //in 4 days
        //tomorrow
        //4 days time
        //3 months time
        //FY13/14
        //FY13-14
        //FY2013-14
        //FY 2013 2014
        //FY13
        //FY 2013


        //alternative notations for time (+postfix "s" for plural):
        //day/d
        //week/wk/w
        //month/mon/mth/mnth/m
        //year/yr/y


        //N time 'ago'
        //^\s*(\d+)\s+(day|week|month|year)s?\s+ago\s*$

        //'last'/'next' time
        //'last'/'next' N time
        //^\s*(last|next)\s*(\d+)?\s*(week|month|year|FY)s?\s*$

        //FY
        //^\s*FY\s*(\d{2,4})[ -/\\]*(\d{2,4})?\s*$

        //N time
        //'in' N time
        //N time 'time'
        //^\s*(in)?\s*(\d+)\s+(day|week|month|year)s?\s*(time)?\s*$

        //and of course:
        //'yesterday'
        //'today'
        //'tomorrow'

        private delegate DateValue ParseDelegate(Match m);

        private Dictionary<Regex, ParseDelegate> _matchers;
        public ExtendedDateParser()
        {
            _matchers = new Dictionary<Regex, ParseDelegate>();

            var periods = new List<string>() { 
                "day", "d", 
                "week", "wk", "w", 
                "month", "mon", "mth", "mnth", "m", 
                "year", "yr", "y",
                "decade", "century", "centurie", "millennium", "millennia", "aeon", "eon"
            };
            var periodStr = periods.Aggregate<string>((s1, s2) => String.Format("{0}|{1}", s1, s2));

            _matchers.Add(new Regex("^(today|now)$", RegexOptions.IgnoreCase), m => new DateValue(DateTime.Today, DateTime.Today.AddDays(1)));
            _matchers.Add(new Regex("^tomorrow$", RegexOptions.IgnoreCase), m => new DateValue(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2)));
            _matchers.Add(new Regex("^yesterday$", RegexOptions.IgnoreCase), m => new DateValue(DateTime.Today.AddDays(-1), DateTime.Today));

            // measured in a 1 period in the past
            _matchers.Add(new Regex(String.Concat(@"^(\d+|a|an)\s+(", periodStr, @")s?\s+ago$"), RegexOptions.IgnoreCase), (m) =>
                {
                    int period = 1; // by default 1 day
                    try { period = Int32.Parse(m.Groups[1].Value); }
                    catch { }

                    var d = Calculate(-period, m.Groups[2].Value);
                    return new DateValue(d, d.AddDays(1));
                });

            // measured in a 1 period in the future
            _matchers.Add(new Regex(String.Concat(@"^(in)?\s*(\d+|a|an)\s+(", periodStr, @")s?\s*(time)?$"), RegexOptions.IgnoreCase), (m) =>
                {
                    int period = 1; // by default 1 day
                    try { period = Int32.Parse(m.Groups[2].Value); }
                    catch { }

                    var d = Calculate(period, m.Groups[3].Value);
                    return new DateValue(d, d.AddDays(1));
                });

            // measured in the period defined by user input
            _matchers.Add(new Regex(String.Concat(@"^(in|within)?\s*(the)?\s*(last|next)?\s*(\d+)?\s*(", periodStr, @")s?$"), RegexOptions.IgnoreCase), (m) =>
                {
                    bool past = m.Groups[3].Value.Equals("last");
                    int period = 1;
                    try { period = Int32.Parse(m.Groups[4].Value); }
                    catch { }

                    var d = Calculate((past ? -1 : 1) * period, m.Groups[5].Value);
                    if (past)
                        return new DateValue(d, DateTime.Today);
                    else
                        return new DateValue(DateTime.Today, d);
                });

            // within a financial year range >= 1/7/x & < 1/7/y
            // if only one year range specified, this is the year in which the financial year ENDS
            // use +- 50 years from today when determining 2 digits
            _matchers.Add(new Regex(@"^FY\s*(\d{2,4})[ -/\\]*(\d{2,4})?$", RegexOptions.IgnoreCase), (m) =>
                {
                    int year1 = Int32.Parse(m.Groups[1].Value);
                    if (year1 < 100)
                    {
                        // curYear = 92
                        // year = 02
                        int curYear = DateTime.Today.Year % 100;
                        int curCentury = DateTime.Today.Year - curYear;
                        if (year1 <= curYear && year1 > curYear - 50)
                            year1 += curCentury; // past, this century
                        else if (year1 > curYear + 50 && year1 <= curYear + 100)
                            year1 += (curCentury - 100); // past, last century
                        else if (year1 > curYear && year1 <= curYear + 50)
                            year1 += curCentury; // future, this century
                        else
                            year1 += (curCentury + 100); // future, next century
                    }

                    var d = new DateTime(year1, 7, 1);
                    if (m.Groups[2].Success)
                        return new DateValue(d, d.AddYears(1));
                    return new DateValue(d.AddYears(-1), d);
                });
        }

        private enum Period { day, week, month, year };
        private DateTime Calculate(int increment, string period)
        {
            // I'm going to cheat here as the following logic currently holds true
            var p = Period.day;
            if (period.StartsWith("decade")) { p = Period.year; increment *= 10; }
            else if (period.StartsWith("centur")) { p = Period.year; increment *= 100; }
            else if (period.StartsWith("millen")) { p = Period.year; increment *= 1000; }
            else if (period.Contains("eon")) { p = Period.year; increment *= (10 ^ 9); }
            else if (period.Contains("d")) p = Period.day;
            else if (period.Contains("w")) p = Period.week;
            else if (period.Contains("m")) p = Period.month;
            else if (period.Contains("y")) p = Period.year;

            return Calculate(DateTime.Today, increment, p);
        }

        private DateTime Calculate(DateTime basedate, int increment, Period p)
        {
            if (p == Period.day)
                return basedate.AddDays(increment);
            else if (p == Period.week)
                return basedate.AddDays(increment * 7);
            else if (p == Period.month)
                return basedate.AddMonths(increment);
            else
                return basedate.AddYears(increment);
        }

        protected override object ParseDate(object value)
        {
            if (value is BetweenValue)
            {
                var between = value as BetweenValue;
                var newTween = new BetweenValue();
                newTween.From = between.From == null ? null : ParseDate(between.From);
                newTween.To = between.To == null ? null : ParseDate(between.To);
                return newTween;
            }
            else if (value is string)
            {
                var str = ((string)value).Trim().ToLower();
                foreach (var matcher in _matchers)
                {
                    var match = matcher.Key.Match(str);
                    if (match.Success)
                        return matcher.Value(match);
                }
            }

            return base.ParseDate(value);
        }

        public override object[] Revert(params object[] values)
        {
            // how do we determine "today" vs an actual date
            // how do we know "within 1 week" is today + 7 days?
            throw new NotImplementedException();
        }
    }
}
