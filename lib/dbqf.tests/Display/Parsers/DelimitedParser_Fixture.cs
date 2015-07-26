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
    public class DelimitedParser_Fixture
    {
        [Test]
        public void Comma_delimited()
        {
            var result = new DelimitedParser(",").Parse("123,456");

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("123", result[0]);
            Assert.AreEqual("456", result[1]);
        }

        [Test]
        public void Tab_delimited()
        {
            var result = new DelimitedParser("\t").Parse("123\t4,567");

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("123", result[0]);
            Assert.AreEqual("4,567", result[1]);
        }

        [Test]
        public void Comma_tab_delimited()
        {
            var result = new DelimitedParser(",", "\t").Parse("123,456\t789");

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual("123", result[0]);
            Assert.AreEqual("456", result[1]);
            Assert.AreEqual("789", result[2]);
        }

        [Test]
        public void Multiple_comma_delimited()
        {
            var result = new DelimitedParser(",").Parse("123,456", "abc");

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual("123", result[0]);
            Assert.AreEqual("456", result[1]);
            Assert.AreEqual("abc", result[2]);
        }

        [Test]
        public void Delimited_with_whitespace()
        {
            var result = new DelimitedParser(",").Parse(" 123 ,\n 456 ", " \tabc, def");

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Length);
            Assert.AreEqual("123", result[0]);
            Assert.AreEqual("456", result[1]);
            Assert.AreEqual("abc", result[2]);
            Assert.AreEqual("def", result[3]);
        }
    }
}
