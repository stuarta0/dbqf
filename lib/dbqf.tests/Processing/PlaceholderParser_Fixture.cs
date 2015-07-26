using dbqf.Criterion;
using dbqf.Processing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbqf.core.tests.Processing
{
    [TestFixture]
    public class PlaceholderParser_Fixture
    {
        [Test]
        public void No_placeholders()
        {
            var c = new Chinook();
            var p = new PlaceholderParser();
            var d = p.Parse(c, "Hello, world!");

            Assert.AreEqual(0, d.Count);
        }

        [Test]
        public void One_placeholder()
        {
            var c = new Chinook();
            var p = new PlaceholderParser();
            var d = p.Parse(c, "Hello, [Track.Name]!");

            Assert.AreEqual(1, d.Count);
            Assert.IsTrue(d.ContainsKey("[Track.Name]"));
            Assert.AreEqual(new FieldPath(c.Track["Name"]), d["[Track.Name]"]);
        }

        [Test]
        public void Replace_one_placeholder()
        {
            var v = new Dictionary<string, string>();
            v.Add("[Track.Name]", "world");
            var p = new PlaceholderParser();
            var d = p.Replace(v, "Hello, [Track.Name]!");

            Assert.AreEqual(1, d.Length);
            Assert.AreEqual("Hello, world!", d[0]);
        }

        [Test]
        public void Replace_one_placeholder_two_strings()
        {
            var v = new Dictionary<string, string>();
            v.Add("[Track.Name]", "world");
            v.Add("[Album.Title]", "universe");
            var p = new PlaceholderParser();
            var d = p.Replace(v, "Hello, [Track.Name]!", "Hello, [Album.Title]!");

            Assert.AreEqual(2, d.Length);
            Assert.AreEqual("Hello, world!", d[0]);
            Assert.AreEqual("Hello, universe!", d[1]);
        }

        [Test]
        public void Replace_one_placeholder_no_values()
        {
            var p = new PlaceholderParser();
            var d = p.Replace(new Dictionary<string, string>(), "Hello, [Track.Name]!");

            Assert.AreEqual(1, d.Length);
            Assert.AreEqual("Hello, !", d[0]);
        }

        [Test]
        public void Depth2_placeholder()
        {
            var c = new Chinook();
            var p = new PlaceholderParser();
            var d = p.Parse(c, "Hello, [Track.AlbumId.Title]!");

            Assert.AreEqual(1, d.Count);
            Assert.IsTrue(d.ContainsKey("[Track.AlbumId.Title]"));
            Assert.AreEqual(new FieldPath(c.Track["AlbumId"], c.Album["Title"]), d["[Track.AlbumId.Title]"]);
        }

        [Test]
        public void Replace_depth2_placeholder()
        {
            var v = new Dictionary<string, string>();
            v.Add("[Track.AlbumId.Title]", "album world");
            var p = new PlaceholderParser();
            var d = p.Replace(v, "Hello, [Track.AlbumId.Title]!");

            Assert.AreEqual(1, d.Length);
            Assert.AreEqual("Hello, album world!", d[0]);
        }

        [Test]
        public void Non_existant_placeholder()
        {
            var c = new Chinook();
            var p = new PlaceholderParser();
            var d = p.Parse(c, "Hello, [Does.Not.Exist]!");

            Assert.AreEqual(0, d.Count);
        }

        [Test]
        public void Replace_non_existant_placeholder()
        {
            var v = new Dictionary<string, string>();
            v.Add("[Track.Name]", "world");
            var p = new PlaceholderParser();
            var d = p.Replace(v, "Hello, [Does.Not.Exist]!");

            Assert.AreEqual(1, d.Length);
            Assert.AreEqual("Hello, !", d[0]);
        }

        [Test]
        public void Invalid_path_placeholder()
        {
            var c = new Chinook();
            var p = new PlaceholderParser();
            var d = p.Parse(c, "Hello, [Track.Album.Name]!");

            Assert.AreEqual(0, d.Count);
        }

        [Test]
        public void Multiple_placeholders()
        {
            var c = new Chinook();
            var p = new PlaceholderParser();
            var d = p.Parse(c, "Hello, [Artist.Name]'s [Track.Name]!");

            Assert.AreEqual(2, d.Count);
            Assert.IsTrue(d.ContainsKey("[Artist.Name]"));
            Assert.IsTrue(d.ContainsKey("[Track.Name]"));
            Assert.AreEqual(new FieldPath(c.Artist["Name"]), d["[Artist.Name]"]);
            Assert.AreEqual(new FieldPath(c.Track["Name"]), d["[Track.Name]"]);
        }

        [Test]
        public void Replace_multiple_placeholders()
        {
            var v = new Dictionary<string, string>();
            v.Add("[Artist.Name]", "solar system");
            v.Add("[Track.Name]", "world");
            var p = new PlaceholderParser();
            var d = p.Replace(v, "Hello, [Artist.Name]'s [Track.Name]!");

            Assert.AreEqual(1, d.Length);
            Assert.AreEqual("Hello, solar system's world!", d[0]);
        }
    }
}
