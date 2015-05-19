using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using dbqf.tests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbqf.WinForms.tests
{
    [TestFixture]
    public class FieldPathComboAdapter_Fixture
    {
        private class Factory : IFieldPathFactory
        {
            public IList<FieldPath> GetFields(Configuration.ISubject subject)
            {
                return subject.Convert<IField, FieldPath>(f => new FieldPath(f));
            }
        }


        [Test]
        public void Single_field_path()
        {
            var config = new Chinook();
            var factory = new Factory();
            var adapter = new FieldPathComboAdapter(factory);

            var path = new FieldPath(config["Track"]["Name"]);
            var fields = factory.GetFields(path[0].Subject);

            adapter.SelectedPath = path;
            Assert.AreEqual(1, adapter.ComboSource.Count);
            Assert.AreEqual(fields.Count, adapter.ComboSource[0].Count);
            Assert.Contains(path[0], adapter.ComboSource[0]);
        }

        [Test]
        public void Double_field_path()
        {
            var config = new Chinook();
            var factory = new Factory();
            var adapter = new FieldPathComboAdapter(factory);

            var path = new FieldPath(config["Track"]["AlbumId"], config["Album"]["Title"]);
            var fields1 = factory.GetFields(path[0].Subject);
            var fields2 = factory.GetFields(path[1].Subject);

            adapter.SelectedPath = path;
            Assert.AreEqual(2, adapter.ComboSource.Count);
            Assert.AreEqual(fields1.Count, adapter.ComboSource[0].Count);
            Assert.AreEqual(fields2.Count, adapter.ComboSource[1].Count);
            Assert.Contains(path[0], adapter.ComboSource[0]);
            Assert.Contains(path[1], adapter.ComboSource[1]);
        }

        [Test]
        public void Triple_field_path()
        {
            var config = new Chinook();
            var factory = new Factory();
            var adapter = new FieldPathComboAdapter(factory);

            var path = new FieldPath(config["Track"]["AlbumId"], config["Album"]["ArtistId"], config["Artist"]["Name"]);
            var fields1 = factory.GetFields(path[0].Subject);
            var fields2 = factory.GetFields(path[1].Subject);
            var fields3 = factory.GetFields(path[2].Subject);

            adapter.SelectedPath = path;
            Assert.AreEqual(3, adapter.ComboSource.Count);
            Assert.AreEqual(fields1.Count, adapter.ComboSource[0].Count);
            Assert.AreEqual(fields2.Count, adapter.ComboSource[1].Count);
            Assert.AreEqual(fields3.Count, adapter.ComboSource[2].Count);
            Assert.Contains(path[0], adapter.ComboSource[0]);
            Assert.Contains(path[1], adapter.ComboSource[1]);
            Assert.Contains(path[2], adapter.ComboSource[2]);
        }

        [Test]
        public void Incomplete_field_path()
        {
            var config = new Chinook();
            var factory = new Factory();
            var adapter = new FieldPathComboAdapter(factory);

            var path = new FieldPath(config["Track"]["AlbumId"], config["Album"]["ArtistId"]);
            var fields1 = factory.GetFields(path[0].Subject);
            var fields2 = factory.GetFields(path[1].Subject);
            var fields3 = factory.GetFields(config["Artist"]); // this should be added by SelectedPath

            adapter.SelectedPath = path;
            Assert.AreEqual(3, adapter.ComboSource.Count);
            Assert.AreEqual(fields1.Count, adapter.ComboSource[0].Count);
            Assert.AreEqual(fields2.Count, adapter.ComboSource[1].Count);
            Assert.AreEqual(fields3.Count, adapter.ComboSource[2].Count);
            Assert.Contains(path[0], adapter.ComboSource[0]);
            Assert.Contains(path[1], adapter.ComboSource[1]);
            Assert.Contains(path[2], adapter.ComboSource[2]);
        }
    }
}
