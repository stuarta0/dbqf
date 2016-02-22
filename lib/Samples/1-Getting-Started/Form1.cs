using System;
using System.Windows.Forms;

namespace _1_Getting_Started
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Usually created once for the lifetime of the application.
            // This code will reside in Samples.Common.TestConfiguration for the remaining samples.
            var config = new dbqf.Sql.Configuration.MatrixConfiguration()
                .Subject(new dbqf.Sql.Configuration.SqlSubject("Foo")
                  .SqlQuery("SELECT * FROM [Foo]")
                  .FieldId(new dbqf.Configuration.Field("Id", typeof(int)))
                  .FieldDefault(new dbqf.Configuration.Field("Name", typeof(string)))
                  .Field(new dbqf.Configuration.Field("Total", typeof(int)))
                  .Field(new dbqf.Configuration.Field("DateCreated", "Date Created", typeof(DateTime)))
                  .Field(new dbqf.Configuration.Field("IsArchived", "Is Archived", typeof(bool))));

            // Created any time a PresetView UI is required.
            // Note: the ControlFactory and ParameterBuilderFactory can be reused across the application.
            var preset = new dbqf.WinForms.PresetView(new dbqf.Display.Preset.PresetAdapter<Control>(
                new dbqf.WinForms.UIElements.WinFormsControlFactory(),
                new dbqf.Sql.Criterion.ParameterBuilderFactory()));
            preset.Adapter.SetParts(new dbqf.Display.FieldPathFactory().GetFields(config[0]));
            preset.Dock = DockStyle.Fill;
            this.Controls.Add(preset);

            // Event occurs when the user presses enter in one of the controls.
            preset.Adapter.Search += delegate { MessageBox.Show("Search requested!"); };
        }
    }
}
