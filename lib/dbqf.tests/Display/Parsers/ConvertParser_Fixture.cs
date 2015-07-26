using dbqf.Parsers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbqf.core.tests.Display.Parsers
{
    [TestFixture]
    public class ConvertParser_Fixture
    {
        [Test]
        public void Convert_parse_string()
        {
            var result = new ConvertParser<string, string>().Parse("abc");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("abc", result[0]);
        }

        [Test]
        public void Convert_parse_bool()
        {
            var result = new ConvertParser<string, bool>().Parse("false");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(false, result[0]);
        }

        [Test]
        public void Convert_parse_int()
        {
            var result = new ConvertParser<string, int>().Parse("123");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(123, result[0]);
        }

        [Test]
        public void Convert_parse_double()
        {
            var result = new ConvertParser<string, double>().Parse("123.4");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(123.4, result[0]);
        }

        [Test]
        public void Convert_parse_string_from_int()
        {
            var result = new ConvertParser<int, string>().Parse(123);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("123", result[0]);
        }

        [Test]
        public void Convert_parse_bool_from_bool()
        {
            var result = new ConvertParser<bool, bool>().Parse(true);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(true, result[0]);
        }

        [Test]
        public void Convert_parse_int_from_int()
        {
            var result = new ConvertParser<int, int>().Parse(123);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(123, result[0]);
        }

        [Test]
        public void Convert_parse_double_from_double()
        {
            var result = new ConvertParser<double, double>().Parse(123.4);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(123.4, result[0]);
        }

        [Test]
        public void Convert_parse_double_from_int()
        {
            var result = new ConvertParser<double, int>().Parse(123.4);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(123, result[0]);
        }
    }
}
