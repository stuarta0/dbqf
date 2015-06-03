using dbqf.Display;
using dbqf.Parsers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbqf.tests.Display.Parsers
{
    [TestFixture]
    public class DateParser_Fixture
    {
        /// <summary>
        /// Shortcut for the number of times a new datetime needs to be created.
        /// </summary>
        private DateTime d(int year, int month, int day)
        {
            return new DateTime(year, month, day);
        }

        private void Check(object values, DateTime d1, DateTime d2)
        {
            Assert.IsInstanceOf<DateValue>(values);
            var dates = (DateValue)values;
            Assert.AreEqual(d1, dates.Lower);
            Assert.AreEqual(d2, dates.Upper);
        }

        private void Check(object values, DateTime d1, DateTime d2, DateTime d3, DateTime d4)
        {
            Assert.IsInstanceOf<BetweenValue>(values);
            var between = (BetweenValue)values;
            Assert.IsInstanceOf<DateValue>(between.From);
            Assert.IsInstanceOf<DateValue>(between.To);
            var from = (DateValue)between.From;
            var to = (DateValue)between.To;

            Assert.AreEqual(d1, from.Lower);
            Assert.AreEqual(d2, from.Upper);
            Assert.AreEqual(d3, to.Lower);
            Assert.AreEqual(d4, to.Upper);
        }

        [Test]
        public void DateTime_object()
        {
            var result = new DateParser().Parse(d(2014, 8, 6));

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Check(result[0], d(2014, 8, 6), d(2014, 8, 7));
        }

        [Test]
        public void Day_dMyyyy()
        {
            var result = new DateParser().Parse("6/8/2014");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Check(result[0], d(2014, 8, 6), d(2014, 8, 7));
        }

        [Test]
        public void Day_dMyy()
        {
            var result = new DateParser().Parse("6/8/14");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Check(result[0], d(2014, 8, 6), d(2014, 8, 7));
        }

        [Test]
        public void Day_dMMMyyyy()
        {
            var result = new DateParser().Parse("6 August 2014");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Check(result[0], d(2014, 8, 6), d(2014, 8, 7));
        }

        [Test]
        public void Day_MMMdyyyy()
        {
            var result = new DateParser().Parse("August 6, 2014");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Check(result[0], d(2014, 8, 6), d(2014, 8, 7));
        }

        [Test]
        public void Day_yyyyMMdd()
        {
            var result = new DateParser().Parse("20140806");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Check(result[0], d(2014, 8, 6), d(2014, 8, 7));
        }

        [Test]
        public void Day_yyyyMMddhhmmss()
        {
            var result = new DateParser().Parse("2014-08-06T21:58");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Check(result[0], d(2014, 8, 6), d(2014, 8, 7));
        }

        [Test]
        public void Month_MMyyyy()
        {
            var result = new DateParser().Parse("08/2014");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Check(result[0], d(2014, 8, 1), d(2014, 9, 1));
        }

        [Test]
        public void Month_MMMyyyy()
        {
            var result = new DateParser().Parse("August 2014");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Check(result[0], d(2014, 8, 1), d(2014, 9, 1));
        }

        [Test]
        public void Month_MMMyyyy_short()
        {
            var result = new DateParser().Parse("Aug 2014");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Check(result[0], d(2014, 8, 1), d(2014, 9, 1));
        }

        [Test]
        public void Year_yyyy()
        {
            var result = new DateParser().Parse("2014");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Check(result[0], d(2014, 1, 1), d(2015, 1, 1));
        }

        [Test]
        public void Two_dates_two_results()
        {
            var result = new DateParser().Parse("July 2014", "Jun 2015");

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Check(result[0], d(2014, 7, 1), d(2014, 8, 1));
            Check(result[1], d(2015, 6, 1), d(2015, 7, 1));
        }
    }
}
