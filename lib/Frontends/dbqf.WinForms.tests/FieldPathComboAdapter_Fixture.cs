using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using dbqf.core.tests;
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
            public IList<IFieldPath> GetFields(Configuration.ISubject subject)
            {
                return subject.Convert<IField, IFieldPath>(f => new FieldPath(f));
            }

            public IList<IFieldPath> GetFields(IRelationField field)
            {
                return GetFields(field.RelatedSubject);
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
            Assert.AreEqual(1, adapter.Items.Count);
            Assert.AreEqual(fields.Count, adapter.Items[0].Fields.Count);
            Assert.Contains(path[0], adapter.Items[0].Fields);
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
            Assert.AreEqual(2, adapter.Items.Count);
            Assert.AreEqual(fields1.Count, adapter.Items[0].Fields.Count);
            Assert.AreEqual(fields2.Count, adapter.Items[1].Fields.Count);
            Assert.Contains(path[0], adapter.Items[0].Fields);
            Assert.Contains(path[1], adapter.Items[1].Fields);
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
            Assert.AreEqual(3, adapter.Items.Count);
            Assert.AreEqual(fields1.Count, adapter.Items[0].Fields.Count);
            Assert.AreEqual(fields2.Count, adapter.Items[1].Fields.Count);
            Assert.AreEqual(fields3.Count, adapter.Items[2].Fields.Count);
            Assert.Contains(path[0], adapter.Items[0].Fields);
            Assert.Contains(path[1], adapter.Items[1].Fields);
            Assert.Contains(path[2], adapter.Items[2].Fields);
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
            Assert.AreEqual(3, adapter.Items.Count);
            Assert.AreEqual(fields1.Count, adapter.Items[0].Fields.Count);
            Assert.AreEqual(fields2.Count, adapter.Items[1].Fields.Count);
            Assert.AreEqual(fields3.Count, adapter.Items[2].Fields.Count);
            Assert.Contains(path[0], adapter.Items[0].Fields);
            Assert.Contains(path[1], adapter.Items[1].Fields);
            Assert.Contains(path[2], adapter.Items[2].Fields);
        }
    }
}
