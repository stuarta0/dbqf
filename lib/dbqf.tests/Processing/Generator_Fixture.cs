using dbqf.Criterion;
using dbqf.Processing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbqf.core.tests.Processing
{
    [TestFixture]
    public class Generator_Fixture
    {
        private void Test(SqlGenerator gen, string sql)
        {
            var cmd = new SqlCommand();
            gen.UpdateCommand(cmd);

            Console.WriteLine(cmd.CommandText);
            var comparer = new dbqf.core.tests.SqlProvider.SqlCommandComparer(Properties.Settings.Default.ConnectionString);
            comparer.Test(cmd.CommandText, sql);
        }

        [Test]
        public void Single_subject_single_field()
        {
            var aw = new AdventureWorks();
            var gen = new SqlGenerator(aw)
                .Target(aw.Products)
                .Column(new FieldPath(aw.Products.IdField));

            Test(gen, @"
SELECT     Production.Product.ProductID
FROM         Production.Product");
        }

        [Test]
        public void Single_subject_single_relation_field()
        {
            var aw = new AdventureWorks();
            var gen = new SqlGenerator(aw)
                .Target(aw.Products)
                .Column(new FieldPath(aw.Products["ProductCategoryID"], aw.ProductCategory.DefaultField));

            Test(gen, @"
SELECT     Production.ProductCategory.Name
FROM         Production.Product 
LEFT OUTER JOIN Production.ProductSubcategory ON Product.ProductSubcategoryID = ProductSubcategory.ProductSubcategoryID
LEFT OUTER JOIN Production.ProductCategory ON ProductSubcategory.ProductCategoryID = Production.ProductCategory.ProductCategoryID");
        }

        [Test]
        public void Single_subject_relation_fields()
        {
            var aw = new AdventureWorks();
            var gen = new SqlGenerator(aw)
                .Target(aw.Products)
                .Column(new FieldPath(aw.Products["ProductCategoryID"], aw.ProductCategory.DefaultField))
                .Column(new FieldPath(aw.Products["ProductSubcategoryID"], aw.ProductSubCategory.DefaultField));
            
            Test(gen, @"
SELECT     Production.ProductCategory.Name, Production.ProductSubcategory.Name
FROM         Production.Product 
LEFT OUTER JOIN Production.ProductSubcategory ON Product.ProductSubcategoryID = ProductSubcategory.ProductSubcategoryID
LEFT OUTER JOIN Production.ProductCategory ON ProductSubcategory.ProductCategoryID = Production.ProductCategory.ProductCategoryID");
        }

        [Test]
        public void Multiple_subject_single_fields()
        {
            var aw = new AdventureWorks();
            var gen = new SqlGenerator(aw)
                .Target(aw.Products)
                .ColumnOrderBy(new FieldPath(aw.Products["Name"]), SqlGenerator.SortDirection.Ascending)
                .ColumnOrderBy(new FieldPath(aw.BillOfMaterials["PerAssemblyQty"]), SqlGenerator.SortDirection.Ascending);

            Test(gen, @"
SELECT     Production.Product.Name, Production.BillOfMaterials.PerAssemblyQty
FROM         Production.Product 
LEFT OUTER JOIN Production.BillOfMaterials ON BillOfMaterials.ProductAssemblyID = Product.ProductID
ORDER BY Product.Name, BillOfMaterials.PerAssemblyQty");
        }

        [Test]
        public void Multiple_subject_relation_fields()
        {
            var aw = new AdventureWorks();

            // first two columns should be identical, third should differ
            var gen = new SqlGenerator(aw)
                .Target(aw.Products)
                .Column(new FieldPath(aw.Products.DefaultField))
                .Column(new FieldPath(aw.BillOfMaterials["ProductAssemblyID"], aw.Products.DefaultField))
                .Column(new FieldPath(aw.BillOfMaterials["ComponentID"], aw.Products.DefaultField));

            // TODO: complete test for this case
        }



        [Test]
        public void Single_subject_equals_operator()
        {
            var aw = new AdventureWorks();
            var gen = new SqlGenerator(aw)
                .Target(aw.Products)
                .Column(new FieldPath(aw.Products.IdField))
                .Where(new SimpleParameter(aw.Products.DefaultField, "=", "Chain"));
                
            Test(gen, @"
SELECT     Production.Product.ProductID
FROM         Production.Product 
WHERE     (Production.Product.Name = N'Chain')");
        }

        [Test]
        public void Single_subject_like_operator()
        {
            var aw = new AdventureWorks();
            var gen = new SqlGenerator(aw)
                .Target(aw.Products)
                .Column(new FieldPath(aw.Products.IdField))
                .Where(new LikeParameter(aw.Products.DefaultField, "Chain"));

            Test(gen, @"
SELECT     Production.Product.ProductID
FROM         Production.Product 
WHERE     (Production.Product.Name LIKE N'%Chain%')");
        }

        [Test]
        public void Single_subject_like_startswith_operator()
        {
            var aw = new AdventureWorks();
            var gen = new SqlGenerator(aw)
                .Target(aw.Products)
                .Column(new FieldPath(aw.Products.IdField))
                .Where(new LikeParameter(aw.Products.DefaultField, "Lock", MatchMode.Start));

            Test(gen, @"
SELECT     Production.Product.ProductID
FROM         Production.Product 
WHERE     (Production.Product.Name LIKE N'Lock%')");
        }

        [Test]
        public void Single_subject_like_endswith_operator()
        {
            var aw = new AdventureWorks();
            var gen = new SqlGenerator(aw)
                .Target(aw.Products)
                .Column(new FieldPath(aw.Products.IdField))
                .Where(new LikeParameter(aw.Products.DefaultField, "Plate", MatchMode.End));

            Test(gen, @"
SELECT     Production.Product.ProductID
FROM         Production.Product 
WHERE     (Production.Product.Name LIKE N'%Plate')");
        }

        [Test]
        public void Single_subject_not_operator()
        {
            var aw = new AdventureWorks();
            var gen = new SqlGenerator(aw)
                .Target(aw.Products)
                .Column(new FieldPath(aw.Products.IdField))
                .Where(new NotParameter(new SimpleParameter(aw.Products.DefaultField, "=", "Chain")));

            Test(gen, @"
SELECT     Production.Product.ProductID
FROM         Production.Product 
WHERE     not (Production.Product.Name = N'Chain')");

            Test(gen, @"
SELECT     Production.Product.ProductID
FROM         Production.Product 
WHERE     Production.Product.Name <> N'Chain'");
        }

        [Test]
        public void Single_subject_conjunction_operator()
        {
            var aw = new AdventureWorks();
            var gen = new SqlGenerator(aw)
                .Target(aw.Products)
                .Column(new FieldPath(aw.Products.IdField))
                .Where(new Conjunction()
                    .Parameter(new SimpleParameter(aw.Products["Color"], "=", "Red"))
                    .Parameter(new SimpleParameter(aw.Products["StandardCost"], ">", 2000)));

            Test(gen, @"
SELECT     Production.Product.ProductID
FROM         Production.Product 
WHERE     Production.Product.Color = N'Red' AND Production.Product.StandardCost > 2000");
        }

        [Test]
        public void Single_subject_empty_conjunction_operator()
        {
            var aw = new AdventureWorks();
            var gen = new SqlGenerator(aw)
                .Target(aw.Products)
                .Column(new FieldPath(aw.Products.IdField))
                .Where(new Conjunction());

            Test(gen, @"
SELECT     Production.Product.ProductID
FROM         Production.Product");
        }

        [Test]
        public void Single_subject_disjunction_operator()
        {
            var aw = new AdventureWorks();
            var gen = new SqlGenerator(aw)
                .Target(aw.Products)
                .Column(new FieldPath(aw.Products.IdField))
                .Where(new Disjunction()
                    .Parameter(new LikeParameter(aw.Products.DefaultField, "Crank"))
                    .Parameter(new LikeParameter(aw.Products.DefaultField, "Washer")));

            Test(gen, @"
SELECT     Production.Product.ProductID
FROM         Production.Product 
WHERE     Production.Product.Name LIKE N'%Crank%' OR Production.Product.Name LIKE N'%Washer%'");
        }

        [Test]
        public void Single_subject_conjunction_with_disjunction()
        {
            var aw = new AdventureWorks();
            var gen = new SqlGenerator(aw)
                .Target(aw.Products)
                .Column(new FieldPath(aw.Products.IdField))
                .Where(new Conjunction()
                    .Parameter(new Disjunction()
                        .Parameter(new LikeParameter(aw.Products.DefaultField, "Crank"))
                        .Parameter(new LikeParameter(aw.Products.DefaultField, "Frame")))
                    .Parameter(new Disjunction()
                        .Parameter(new LikeParameter(aw.Products["Color"], "Black"))
                        .Parameter(new LikeParameter(aw.Products["Color"], "Red"))));

            Test(gen, @"
SELECT     Production.Product.ProductID
FROM         Production.Product 
WHERE     (Production.Product.Name LIKE N'%Crank%' OR Production.Product.Name LIKE N'%Frame%')
		AND (Production.Product.Color = 'Black' OR Production.Product.Color = 'Red')");
        }

        [Test]
        public void Single_subject_empty_conjunction_within_disjunction()
        {
            var aw = new AdventureWorks();
            var gen = new SqlGenerator(aw)
                .Target(aw.Products)
                .Column(new FieldPath(aw.Products.IdField))
                .Where(new Disjunction().Parameter(new Conjunction()));

            Test(gen, @"
SELECT     Production.Product.ProductID
FROM         Production.Product");
        }

        [Test]
        public void Single_subject_null_operator()
        {
            var aw = new AdventureWorks();
            var gen = new SqlGenerator(aw)
                .Target(aw.Products)
                .Column(new FieldPath(aw.Products.IdField))
                .Where(new NullParameter(aw.Products["Color"]));

            Test(gen, @"
SELECT     Production.Product.ProductID
FROM         Production.Product 
WHERE     Production.Product.Color IS NULL");
        }

        [Test]
        public void Single_subject_between_operator_double()
        {
            var aw = new AdventureWorks();
            var gen = new SqlGenerator(aw)
                .Target(aw.Products)
                .Column(new FieldPath(aw.Products.IdField))
                .Where(new BetweenParameter(aw.Products["StandardCost"], 50, 100));

            Test(gen, @"
SELECT     Production.Product.ProductID
FROM         Production.Product 
WHERE     Production.Product.StandardCost BETWEEN 50 AND 100");
        }

        [Test]
        public void Single_subject_between_operator_date()
        {
            var aw = new AdventureWorks();
            var gen = new SqlGenerator(aw)
                .Target(aw.Products)
                .Column(new FieldPath(aw.Products.IdField))
                .Where(new BetweenParameter(aw.Products["SellStartDate"],
                    new DateTime(2006, 1, 1),
                    new DateTime(2006, 12, 31)));

            Test(gen, @"
SELECT     Production.Product.ProductID
FROM         Production.Product 
WHERE     Production.Product.SellStartDate BETWEEN '20060101' AND '20061231'");
        }
    }
}
