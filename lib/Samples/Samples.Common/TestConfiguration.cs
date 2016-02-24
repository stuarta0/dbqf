using dbqf.Configuration;
using dbqf.Sql.Configuration;
using System;

namespace Samples.Common
{
    /// <summary>
    /// An example configuration used throughout the sample code.
    /// </summary>
    public class TestConfiguration : MatrixConfiguration
    {
        public TestConfiguration()
        {
            SqlSubject foo, bar;

            // Add a subject to this configuration with some basic fields.
            this.Subject(foo = new SqlSubject("Foo")
                  .SqlQuery("SELECT * FROM [Foo]")
                  .FieldId(new Field("Id", typeof(int)))
                  .FieldDefault(new Field("Name", typeof(string)))
                  .Field(new Field("Total", typeof(int)))
                  .Field(new Field("DateCreated", "Date Created", typeof(DateTime)))
                  .Field(new Field("IsArchived", "Is Archived", typeof(bool))));

            // Add another subject, this time with a relationship to the first subject.
            this.Subject(bar = new SqlSubject("Bar")
                .SqlQuery("SELECT * FROM [Bar]")
                .FieldId(new Field("Id", typeof(int)))
                .FieldDefault(new Field("Name", typeof(string)))
                .Field(new RelationField("FooId", "My Foo", foo))
                .Field(new Field("Status", typeof(string)) {
                    List = new FieldList(new object[] { "Pending", "Reviewed", "Active", "Cancelled" })
                }));

            // To ensure ad-hoc relationships can be created dynamically by the user 
            // we need to define how we traverse from Bar to Foo and vice-versa.
            // (e.g. Search for Bar where Foo.Name = 'Hello World')
            this.Matrix(bar, foo, "SELECT Bar.Id FromId, Bar.FooId ToId FROM [Bar]")
                .Matrix(foo, bar, "SELECT Bar.FooId FromId, Bar.Id ToId FROM [Bar]");
        }
    }
}
