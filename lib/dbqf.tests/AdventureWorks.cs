using dbqf.Configuration;
using dbqf.Sql.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbqf.core.tests
{
    public sealed class AdventureWorks : MatrixConfiguration
    {
        private ISqlSubject _products, _materials, _category, _subCategory, _contacts, _sales, _salesDetails;

        public AdventureWorks()
        {
            this
                .Subject(Products)
                .Subject(BillOfMaterials)
                .Subject(ProductCategory)
                .Subject(ProductSubCategory)
                .Subject(Contacts)
                .Subject(SalesOrders)
                .Subject(SalesOrderDetails)

                .Matrix(Products, Products, "SELECT ProductID AS FromID, ProductID AS ToID FROM Production.Products", "Search for products")
                .Matrix(Products, BillOfMaterials, "SELECT [ProductAssemblyID] AS FromID, [BillOfMaterialsID] AS ToID FROM Production.BillOfMaterials", "Search for components that make up a product")
                .Matrix(Products, ProductCategory, "SELECT [Product].[ProductID] AS FromID, [ProductCategory].[ProductCategoryID] AS ToID FROM Production.[Product] INNER JOIN Production.[ProductSubcategory] ON [Product].[ProductSubcategoryID] = [ProductSubcategory].[ProductSubcategoryID] INNER JOIN Production.[ProductCategory] ON [ProductSubcategory].[ProductCategoryID] = [ProductCategory].[ProductCategoryID]", "Search for categories that contain a product")
                .Matrix(Products, ProductSubCategory, "SELECT [ProductID] AS FromID, [ProductSubcategoryID] AS ToID FROM Production.[Product]", "Search for subcategories that contain a product")
                .Matrix(Products, Contacts, "", "")
                .Matrix(Products, SalesOrders, "SELECT [Product].[ProductID] AS FromID, [SalesOrderHeader].[SalesOrderID] AS ToID FROM Production.[Product] INNER JOIN Sales.[SalesOrderDetail] ON [Product].[ProductID] = [SalesOrderDetail].[ProductID] INNER JOIN Sales.[SalesOrderHeader] ON [SalesOrderDetail].[SalesOrderID] = [SalesOrderHeader].[SalesOrderID]", "Search for orders with a product")
                .Matrix(Products, SalesOrderDetails, "SELECT [ProductID] AS FromID, [SalesOrderDetailID] AS ToID FROM Sales.[SalesOrderDetail]", "Search for order details for a product")

                .Matrix(BillOfMaterials, Products, "SELECT [BillOfMaterialsID] AS FromID, [ProductAssemblyID] AS ToID FROM Production.BillOfMaterials", "Search for products made up of components")
                .Matrix(BillOfMaterials, BillOfMaterials, "SELECT BillOfMaterialsID AS FromID, BillOfMaterialsID AS ToID FROM Production.BillOfMaterials", "Search for components")
                .Matrix(BillOfMaterials, ProductCategory, "", "")
                .Matrix(BillOfMaterials, ProductSubCategory, "", "")
                .Matrix(BillOfMaterials, Contacts, "", "")
                .Matrix(BillOfMaterials, SalesOrders, "", "")
                .Matrix(BillOfMaterials, SalesOrderDetails, "", "")

                .Matrix(ProductCategory, Products, "SELECT [ProductCategory].[ProductCategoryID] AS FromID, [Product].[ProductID] AS ToID FROM Production.[ProductCategory] INNER JOIN Production.[ProductSubcategory] ON [ProductCategory].[ProductCategoryID] = [ProductSubcategory].[ProductCategoryID] INNER JOIN Production.[Product] ON [ProductSubcategory].[ProductSubcategoryID] = [Product].[ProductSubcategoryID]", "Search for products in a category")
                .Matrix(ProductCategory, BillOfMaterials, "", "")
                .Matrix(ProductCategory, ProductCategory, "SELECT ProductCategoryID AS FromID, ProductCategoryID AS ToID FROM Production.ProductCategory", "Search for categories")
                .Matrix(ProductCategory, ProductSubCategory, "SELECT [ProductCategoryID] AS FromID, [ProductSubcategoryID] AS ToID FROM Production.[ProductSubcategory]", "Search for subcategories in a category")
                .Matrix(ProductCategory, Contacts, "", "")
                .Matrix(ProductCategory, SalesOrders, "", "")
                .Matrix(ProductCategory, SalesOrderDetails, "", "")

                .Matrix(ProductSubCategory, Products, "SELECT [ProductSubcategoryID] AS FromID, [ProductID] AS ToID FROM Production.[Product]", "Search for products with a subcategory")
                .Matrix(ProductSubCategory, BillOfMaterials, "", "")
                .Matrix(ProductSubCategory, ProductCategory, "SELECT [ProductSubcategoryID] AS FromID, [ProductCategoryID] AS ToID FROM Production.[ProductSubcategory]", "Search for categories that have a subcategory")
                .Matrix(ProductSubCategory, ProductSubCategory, "SELECT ProductSubcategoryID AS FromID, ProductSubcategoryID AS ToID FROM Production.ProductSubcategory", "Search for subcategories")
                .Matrix(ProductSubCategory, Contacts, "", "")
                .Matrix(ProductSubCategory, SalesOrders, "", "")
                .Matrix(ProductSubCategory, SalesOrderDetails, "", "")

                .Matrix(Contacts, Products, "", "")
                .Matrix(Contacts, BillOfMaterials, "", "")
                .Matrix(Contacts, ProductCategory, "", "")
                .Matrix(Contacts, ProductSubCategory, "", "")
                .Matrix(Contacts, Contacts, "", "")
                .Matrix(Contacts, SalesOrders, "", "")
                .Matrix(Contacts, SalesOrderDetails, "", "")

                .Matrix(SalesOrders, Products, "SELECT [SalesOrderHeader].[SalesOrderID] AS FromID, [Product].[ProductID] AS ToID FROM Sales.[SalesOrderHeader] INNER JOIN Sales.[SalesOrderDetail] ON [SalesOrderHeader].[SalesOrderID] = [SalesOrderDetail].[SalesOrderID] INNER JOIN Production.[Product] ON [SalesOrderDetail].[ProductID] = [Product].[ProductID]", "Search for products on an order")
                .Matrix(SalesOrders, BillOfMaterials, "", "")
                .Matrix(SalesOrders, ProductCategory, "", "")
                .Matrix(SalesOrders, ProductSubCategory, "", "")
                .Matrix(SalesOrders, Contacts, "", "")
                .Matrix(SalesOrders, SalesOrders, "", "")
                .Matrix(SalesOrders, SalesOrderDetails, "SELECT [SalesOrderID] AS FromID, SalesOrderDetailID AS ToID FROM Sales.[SalesOrderDetail]", "Search for sales order details on a sales order")

                .Matrix(SalesOrderDetails, Products, "", "")
                .Matrix(SalesOrderDetails, BillOfMaterials, "", "")
                .Matrix(SalesOrderDetails, ProductCategory, "", "")
                .Matrix(SalesOrderDetails, ProductSubCategory, "", "")
                .Matrix(SalesOrderDetails, Contacts, "", "")
                .Matrix(SalesOrderDetails, SalesOrders, "SELECT SalesOrderDetailID AS FromID, [SalesOrderID] AS ToID FROM Sales.[SalesOrderDetail]", "Search for sales orders with details")
                .Matrix(SalesOrderDetails, SalesOrderDetails, "", "")
                ;
        }

        public ISqlSubject Products
        {
            get
            {
                if (_products == null)
                {
                    var s = new SqlSubject("Products")
                        .SqlQuery(@"
SELECT ProductID AS ID, Product.Name, ProductNumber, Color, StandardCost, ListPrice, 
Product.ProductSubcategoryID, ProductSubcategory.Name AS SubcategoryName, 
ProductCategory.ProductCategoryID, ProductCategory.Name AS CategoryName,
ProductModelID, SellStartDate, SellEndDate, FinishedGoodsFlag
FROM Production.Product
LEFT OUTER JOIN Production.ProductSubcategory ON Product.ProductSubcategoryID = ProductSubcategory.ProductSubcategoryID
LEFT OUTER JOIN Production.ProductCategory ON ProductSubcategory.ProductCategoryID = ProductCategory.ProductCategoryID")
                        .FieldId(new Field("id", typeof(int)))
                        .Field(new Field("ProductNumber", "Number", typeof(string)))
                        .FieldDefault(new Field("Name", typeof(string)))
                        .Field(new RelationField("ProductCategoryID", "Category", ProductCategory))
                        .Field(new RelationField("ProductSubcategoryID", "Subcategory", ProductSubCategory))
                        .Field(new Field("Color", "Colour", typeof(string)))
                        .Field(new Field("StandardCost", "Standard Cost", typeof(double)) { DisplayFormat = "C2" })
                        .Field(new Field("ListPrice", "List Price", typeof(double)) { DisplayFormat = "C2" })
                        .Field(new Field("SellStartDate", "Sell Start Date", typeof(DateTime)) { DisplayFormat = "dd/MM/yyyy" })
                        .Field(new Field("SellEndDate", "Sell End Date", typeof(DateTime)) { DisplayFormat = "dd/MM/yyyy" })
                        .Field(new Field("FinishedGoodsFlag", "Is Finished Goods", typeof(bool)));
                    _products = s;
                }

                return _products;
            }
        }

        public ISqlSubject BillOfMaterials
        {
            get
            {
                if (_materials == null)
                {
                    var s = new SqlSubject("Bill Of Materials")
                        .SqlQuery(@"
SELECT BillOfMaterialsID AS ID, ProductAssemblyID, 
ComponentID, StartDate, EndDate, UnitMeasureCode, BOMLevel, PerAssemblyQty
FROM Production.BillOfMaterials")
                        .FieldId(new Field("id", typeof(int)))
                        .FieldDefault(new RelationField("ProductAssemblyID", "Product", Products))
                        .Field(new RelationField("ComponentID", "Component", Products))
                        .Field(new Field("PerAssemblyQty", "Quantity", typeof(double)) { DisplayFormat = "0.##" })
                        .Field(new Field("StartDate", "Start Date", typeof(DateTime)) { DisplayFormat = "dd/MM/yyyy" })
                        .Field(new Field("EndDate", "End Date", typeof(DateTime)) { DisplayFormat = "dd/MM/yyyy" });
                    _materials = s;
                }

                return _materials;
            }
        }

        public ISqlSubject ProductCategory
        {
            get
            {
                if (_category == null)
                {
                    var s = new SqlSubject("Product Category")
                        .SqlQuery("SELECT ProductCategoryID AS ID, Name FROM Production.ProductCategory")
                        .FieldId(new Field("id", typeof(int)))
                        .FieldDefault(new Field("Name", typeof(string)) 
                        { 
                            List = new FieldList() 
                            { 
                                Source = "SELECT ProductCategoryID AS ID, Name FROM Production.ProductCategory", 
                                Type = FieldListType.Limited
                            } 
                        });
                    _category = s;
                }

                return _category;
            }
        }

        public ISqlSubject ProductSubCategory
        {
            get
            {
                if (_subCategory == null)
                {
                    var s = new SqlSubject("Product Subcategory")
                        .SqlQuery("SELECT ProductSubcategoryID AS ID, Name FROM Production.ProductSubcategory")
                        .FieldId(new Field("id", typeof(int)))
                        .FieldDefault(new Field("Name", typeof(string)));
                    _subCategory = s;
                }

                return _subCategory;
            }
        }

        public ISqlSubject Contacts
        {
            get
            {
                if (_contacts == null)
                {
                    var s = new SqlSubject("Contact")
                        .SqlQuery("SELECT ContactID AS ID, CAST(EmailPromotion AS bit) AS Promo, * FROM Person.Contact")
                        .FieldId(new Field("id", typeof(int)))
                        .Field(new Field("Title", typeof(string)))
                        .FieldDefault(new Field("FirstName", "First Name", typeof(string)))
                        .Field(new Field("LastName", "Last Name", typeof(string)))
                        .Field(new Field("EmailAddress", "Email", typeof(string)))
                        .Field(new Field("Phone", typeof(string)))
                        .Field(new Field("Promo", "Receives Promotions", typeof(bool)));
                    _contacts = s;
                }

                return _contacts;
            }
        }

        public ISqlSubject SalesOrders
        {
            get
            {
                if (_sales == null)
                {
                    var s = new SqlSubject("Sales Orders")
                        .SqlQuery("SELECT SalesOrderID AS ID, SalesOrderHeader.* FROM Sales.SalesOrderHeader")
                        .FieldId(new Field("id", typeof(int)))
                        .FieldDefault(new Field("SalesOrderNumber", "Number", typeof(string)))
                        .Field(new Field("Status", typeof(string)))
                        .Field(new Field("CustomerID", "Customer", typeof(int)))
                        .Field(new Field("SalesPersonID", "Sales Person", typeof(int)))
                        .Field(new Field("OrderDate", "Order Date", typeof(DateTime)) { DisplayFormat = "dd/MM/yyyy" })
                        .Field(new Field("DueDate", "Due Date", typeof(DateTime)) { DisplayFormat = "dd/MM/yyyy" })
                        .Field(new Field("ShipDate", "Date Shipped", typeof(DateTime)) { DisplayFormat = "dd/MM/yyyy" })
                        .Field(new Field("SubTotal", "Subtotal", typeof(double)) { DisplayFormat = "C2" })
                        .Field(new Field("TaxAmt", "Tax Amount", typeof(double)) { DisplayFormat = "C2" })
                        .Field(new Field("Freight", "Freight Cost", typeof(double)) { DisplayFormat = "C2" })
                        .Field(new Field("TotalDue", "Total Cost", typeof(double)) { DisplayFormat = "C2" })
                        .Field(new Field("Comment", typeof(string)));
                    _sales = s;
                }

                return _sales;
            }
        }

        public ISqlSubject SalesOrderDetails
        {
            get
            {
                if (_salesDetails == null)
                {
                    var s = new SqlSubject("Sales Orders Details")
                        .SqlQuery(@"
SELECT SalesOrderDetailID AS ID, SalesOrderHeader.SalesOrderNumber, SalesOrderDetail.* 
FROM Sales.SalesOrderDetail 
INNER JOIN Sales.SalesOrderHeader ON SalesOrderDetail.SalesOrderID = SalesOrderHeader.SalesOrderID")
                        .FieldId(new Field("id", typeof(string)))
                        .Field(new RelationField("SalesOrderID", "Sales Order", SalesOrders))
                        .FieldDefault(new RelationField("ProductID", "Product", Products))
                        .Field(new Field("CarrierTrackingNumber", "Tracking Number", typeof(string)))
                        .Field(new Field("OrderQty", "Quantity", typeof(int)))
                        .Field(new Field("UnitPrice", "Unit Price", typeof(double)) { DisplayFormat = "C2" })
                        .Field(new Field("UnitPriceDiscount", "Unit Discount", typeof(double)) { DisplayFormat = "C2" })
                        .Field(new Field("LineTotal", "Total", typeof(double)) { DisplayFormat = "C2" });
                    _salesDetails = s;
                }

                return _salesDetails;
            }
        }
    }
}
