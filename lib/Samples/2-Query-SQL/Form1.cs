using Samples.Common;
using System;
using System.Text;
using System.Windows.Forms;

namespace _2_Query_SQL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSimple_Click(object sender, System.EventArgs e)
        {
            // Using our common FooBar configuration from Samples.Common.
            var config = new TestConfiguration();

            // We need a command to populate CommandText. This can be any type implementing IDbCommand.
            var cmd = new System.Data.SqlClient.SqlCommand();

            // Using the builtin SqlGenerator we can construct our SQL.
            new dbqf.Sql.SqlGenerator(config)

                // Add our Bar Name field, using the provided FieldPath object's FromDefault to initialise the field (result is simply Bar.Name).
                .Column(dbqf.Criterion.FieldPath.FromDefault(config["Bar"]["Name"]))

                // As with any SQL we need to know what we're searching for - in this case we're searching for Bars.
                .ForTarget((dbqf.Sql.Configuration.ISqlSubject)config["Bar"])

                // And now generate the SQL; adding the joins required to represent the columns and targets given.
                .UpdateCommand(cmd);

            // Finally, update our TextBox to view the generated SQL.
            UpdateSql("Simple SQL statement:", cmd);
        }


        private void btnJoin_Click(object sender, System.EventArgs e)
        {
            var config = new TestConfiguration();
            var cmd = new System.Data.SqlClient.SqlCommand();
            new dbqf.Sql.SqlGenerator(config)
                .Column(dbqf.Criterion.FieldPath.FromDefault(config["Bar"]["Name"]))

                // Add our related field 'Bar My Foo'. In this case, FromDefault will generate a path of Bar.FooId -> Foo.Name (since Foo's default field is Name).
                // and generating the SQL will create a join between Bar and Foo.
                .Column(dbqf.Criterion.FieldPath.FromDefault(config["Bar"]["FooId"]))

                .ForTarget((dbqf.Sql.Configuration.ISqlSubject)config["Bar"])
                .UpdateCommand(cmd);

            UpdateSql("SQL with joins:", cmd);
        }

        private void btnParameterized_Click(object sender, System.EventArgs e)
        {
            var config = new TestConfiguration();
            var cmd = new System.Data.SqlClient.SqlCommand();
            new dbqf.Sql.SqlGenerator(config)
                .Column(dbqf.Criterion.FieldPath.FromDefault(config["Bar"]["Name"]))
                .Column(dbqf.Criterion.FieldPath.FromDefault(config["Bar"]["FooId"]))
                .ForTarget((dbqf.Sql.Configuration.ISqlSubject)config["Bar"])

                // Now we'll add some parameters to filter our results; in this case [WHERE Bar.Name LIKE "%Hello World%" AND Foo.DateCreated < "2000-01-01"].
                // Since the columns have already included a join from Bar to Foo and the SimpleParameter below uses the same edge traversal, the 
                // generated SQL will ensure the WHERE clause uses the same joined result.
                .WithWhere(new dbqf.Sql.Criterion.SqlConjunction()
                    .Parameter(new dbqf.Sql.Criterion.SqlLikeParameter(config["Bar"]["Name"], "Hello World"))
                    .Parameter(new dbqf.Sql.Criterion.SqlSimpleParameter(
                        new dbqf.Criterion.FieldPath(config["Bar"]["FooId"], config["Foo"]["DateCreated"]),
                        "<", DateTime.Today))
                )
                
                .UpdateCommand(cmd);
            
            UpdateSql("Parameterized SQL with joins:", cmd);
        }


        private void UpdateSql(string prefix, System.Data.SqlClient.SqlCommand cmd)
        {
            var sql = cmd.CommandText;
            foreach (System.Data.SqlClient.SqlParameter p in cmd.Parameters)
                sql = sql.Replace(p.ParameterName, p.Value.ToString());
            txtSql.Text = prefix + Environment.NewLine + sql;
        }
    }
}
