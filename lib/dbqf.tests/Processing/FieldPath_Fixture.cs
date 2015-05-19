using dbqf.Criterion;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbqf.tests.Processing
{
    [TestFixture]
    public class FieldPath_Fixture
    {
        [Test]
        public void Single_field_path()
        {
            var aw = new AdventureWorks();
            var path = new FieldPath(aw.Products.DefaultField);

            Assert.AreEqual(1, path.Count);
            Assert.AreEqual(aw.Products.DefaultField, path[0]);
        }

        [Test]
        public void Multiple_field_path()
        {
            var aw = new AdventureWorks();
            var path = new FieldPath(
                aw.SalesOrderDetails["ProductID"], 
                aw.Products["ProductCategoryID"], 
                aw.ProductCategory.DefaultField);

            Assert.AreEqual(3, path.Count);
            Assert.AreEqual(aw.SalesOrderDetails["ProductID"], path[0]);
            Assert.AreEqual(aw.Products["ProductCategoryID"], path[1]);
            Assert.AreEqual(aw.ProductCategory.DefaultField, path[2]);
        }

        [Test]
        public void Invalid_add_field_path()
        {
            var aw = new AdventureWorks();
            var path = new FieldPath(aw.Products.DefaultField);

            // no link between normal field product.name and normal field contact.name
            Assert.Throws<InvalidOperationException>(() => { path.Add(aw.Contacts.DefaultField); });
        }

        [Test]
        public void Invalid_add_relation_field_path()
        {
            var aw = new AdventureWorks();
            var path = new FieldPath(aw.Products["ProductCategoryID"]);

            // no link between relation field product.category and normal field contact.name
            Assert.Throws<ArgumentException>(() => { path.Add(aw.Contacts.DefaultField); });
        }

        [Test]
        public void Remove_field_path()
        {
            var aw = new AdventureWorks();
            var path = new FieldPath(
                aw.SalesOrderDetails["ProductID"], 
                aw.Products["ProductCategoryID"], 
                aw.ProductCategory.DefaultField);

            path.Remove(aw.ProductCategory.DefaultField);
            Assert.AreEqual(2, path.Count);
            Assert.AreEqual(aw.SalesOrderDetails["ProductID"], path[0]);
            Assert.AreEqual(aw.Products["ProductCategoryID"], path[1]);
        }

        [Test]
        public void Invalid_remove_relation_field_path()
        {
            var aw = new AdventureWorks();
            var path = new FieldPath(
                aw.SalesOrderDetails["ProductID"], 
                aw.Products["ProductCategoryID"], 
                aw.ProductCategory.DefaultField);

            // removing the middle relation creates an invalid link between sales order.product -> category.name
            Assert.Throws<ArgumentException>(() => { path.Remove(aw.Products["ProductCategoryID"]); });
        }

        [Test]
        public void FromDefault_field_path()
        {
            var aw = new AdventureWorks();
            aw.Products.DefaultField = aw.Products["ProductCategoryID"];

            var path = FieldPath.FromDefault(aw.Products.DefaultField);

            // should be product.category -> category.name
            Assert.AreEqual(2, path.Count);
            Assert.AreEqual(aw.Products["ProductCategoryID"], path[0]);
            Assert.AreEqual(aw.ProductCategory["Name"], path[1]);
        }

        [Test]
        public void Clear_field_path()
        {
            var aw = new AdventureWorks();
            var path = new FieldPath(
                aw.SalesOrderDetails["ProductID"],
                aw.Products["ProductCategoryID"],
                aw.ProductCategory.DefaultField);

            path.Clear();
            Assert.AreEqual(0, path.Count);
        }

        [Test]
        public void Insert0_field_path()
        {
            var aw = new AdventureWorks();
            var path = new FieldPath();

            // creating the path in reverse should be fine
            path.Insert(0, aw.ProductCategory.DefaultField);
            path.Insert(0, aw.Products["ProductCategoryID"]);
            path.Insert(0, aw.SalesOrderDetails["ProductID"]);
        }
    }
}
