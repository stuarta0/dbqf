using dbqf.Display.Parsers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbqf.tests.Display.Parsers
{
    [TestFixture]
    public class ChainedParser_Fixture
    {
        [Test]
        public void Chained_none()
        {
            var result = new ChainedParser().Parse("abc", 123);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("abc", result[0]);
            Assert.AreEqual(123, result[1]);
        }

        [Test]
        public void Chained_convert()
        {
            var result = new ChainedParser()
                .Add(new ConvertParser<string, string>())
                .Parse("abc");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("abc", result[0]);
        }

        [Test]
        public void Chained_delimited_convert()
        {
            var result = new ChainedParser()
                .Add(new DelimitedParser(","))
                .Add(new ConvertParser<string, int>())
                .Parse("123, 456, 789");

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual(123, result[0]);
            Assert.AreEqual(456, result[1]);
            Assert.AreEqual(789, result[2]);
        }

        [Test]
        public void Chained_delimited_convert_revert()
        {
            var result = new ChainedParser()
                .Add(new DelimitedParser(","))
                .Add(new ConvertParser<string, int>())
                .Revert(123, 456, 789);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("123,456,789", result[0]);
        }
    }
}
