using System;
using System.Windows.Forms;

namespace _1_Getting_Started
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Created once for the lifetime of the application.
            var config = new dbqf.Sql.Configuration.MatrixConfiguration()
              .Subject((dbqf.Configuration.ISqlSubject)new dbqf.Sql.Configuration.SqlSubject("Test")
                  .SqlQuery("SELECT * FROM [Test]")
                  .FieldId(new dbqf.Configuration.Field("Id", typeof(int)))
                  .FieldDefault(new dbqf.Configuration.Field("Name", typeof(string)))
                  .Field(new dbqf.Configuration.Field("Total", typeof(int)))
                  .Field(new dbqf.Configuration.Field("Date Created", typeof(DateTime)))
                  .Field(new dbqf.Configuration.Field("Is Archived", typeof(bool))));

            // Created any time a PresetView UI is required.
            // Note: the ControlFactory and ParameterBuilderFactory can be reused across the application.
            var preset = new dbqf.WinForms.PresetView(new dbqf.Display.Preset.PresetAdapter<Control>(
                new dbqf.WinForms.UIElements.WinFormsControlFactory(),
                new dbqf.Display.ParameterBuilderFactory()));
            preset.Adapter.SetParts(new dbqf.Display.FieldPathFactory().GetFields(config[0]));
            preset.Dock = DockStyle.Fill;
            this.Controls.Add(preset);
        }
    }
}
