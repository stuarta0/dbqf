using dbqf.Display;
using dbqf.Display.Preset;
using dbqf.Parsers;
using dbqf.Sql;
using dbqf.Sql.Configuration;
using dbqf.Sql.Criterion;
using dbqf.Sql.Criterion.Builders;
using dbqf.WinForms;
using dbqf.WinForms.UIElements;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace _3_Get_Results
{
    public partial class Form1 : Form
    {
        private MatrixConfiguration _config;
        private PresetAdapter<Control> _adapter;
        public Form1()
        {
            InitializeComponent();

            // Instantiate our test configuration
            _config = new Samples.Common.TestConfiguration();

            // Set up the preset view
            var view = new PresetView(
                _adapter = new PresetAdapter<Control>(
                    new WinFormsControlFactory(), 
                    new ParameterBuilderFactory()
                ));

            // The following code is possibly a smell. The builtin ParameterBuilders that work with
            // dates require DateValues to be parsed from the UI controls. The default control returns 
            // strings so it won't be able to create an SQL parameter. To overcome this, we monitor
            // when a new part is added to the PresetView (occurs when we call SetParts) and initialise
            // a date parser to do this for us.
            _adapter.PartCreated += (s, e) =>
            {
                var junction = e.Part.SelectedBuilder as JunctionBuilder;
                if ((e.Part.SelectedBuilder is IDateParameterBuilder) 
                    || (junction != null && junction.Other is IDateParameterBuilder))
                    e.Part.Parser = new DateParser();
            };

            // Now create controls and add to form
            _adapter.SetParts(new FieldPathFactory().GetFields(_config[0]));
            view.Dock = DockStyle.Fill;
            splitContainer1.Panel1.Controls.Add(view);

            // Hook search requested event from the adapter
            // Usually triggered by pressing enter in one of the fields
            _adapter.Search += Adapter_Search;
        }

        /// <summary>
        /// Occurs when a search is requested from the preset view.
        /// </summary>
        private void Adapter_Search(object sender, EventArgs e)
        {
            // For this example, we're just getting results of the first subject in our configuration
            var target = (ISqlSubject)_config[0];

            // Instantiate an SqlGenerator which will be used to create our SQL
            // (the following fluent calls can be called in any order before the UpdateCommand function is invoked)
            var generator = new SqlGenerator(_config)

                // Call fluent method to add a collection of columns to get
                .Column(new FieldPathFactory().GetFields(target))

                // Set what subject we're primarily searching for
                .ForTarget(target)

                // The most important part: adding the user provided parameters.
                // Since we initialised the PresetAdapter with a ParameterBuilderFactory from the dbqf.Sql
                // namespace, we know the generated IParameter returned from _adapter.GetParameter() will
                // be of the correct ISqlParameter types
                .WithWhere((ISqlParameter)_adapter.GetParameter());

            // Since we're creating our application using MSSQL, we'll use the SqlClient objects
            using (var conn = new SqlConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    // Finally the generator will update our command object with the correct parameterized SQL
                    generator.UpdateCommand(cmd);
                    Console.WriteLine(cmd.CommandText);

                    using (var sda = new SqlDataAdapter(cmd))
                    {
                        // Use a vanilla DataTable bound to the DataGridView
                        var table = new DataTable();
                        dataGridView1.DataSource = table;

                        // This call would finally get our results
                        // (but since this is an example we have no database to query)
                        //sda.Fill(table);

                        // Pretend to get results
                        Fill(table, target);
                    }
                }
            }

        }

        private void Fill(DataTable table, ISqlSubject target)
        {
            foreach (var field in target)
                table.Columns.Add(field.DisplayName);

            int intCount = 0;
            DateTime dateCount = DateTime.Today;
            int stringCount = 0;
            for (var i = 0; i < 5; i++)
            {
                var row = table.NewRow();
                foreach (var field in target)
                {
                    if (field.DataType == typeof(int))
                        row[field.DisplayName] = intCount++;
                    else if (field.DataType == typeof(DateTime))
                        row[field.DisplayName] = (dateCount = dateCount.AddDays(1));
                    else if (field.DataType == typeof(bool))
                        row[field.DisplayName] = true;
                    else
                        row[field.DisplayName] = ((char)(65 + (stringCount++ % 26))).ToString();
                }
                table.Rows.Add(row);
            }
        }
    }
}
