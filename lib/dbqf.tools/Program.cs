using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDesk.Options;
using System.Xml.Serialization;
using System.Data.SqlClient;

namespace dbqf.tools
{
    class Program
    {
        static void Main(string[] args)
        {
            string connection = "";
            bool exportProject = false;
            int listIf = 0;
            bool listDefault = false;
            string order = "";
            string exclude = "";
            var options = new OptionSet() {
                { "connection=", "Connection to an SQL Server database that will be used to generate a configuration",
                    v => connection = v },
                { "as-project", "Output a project object for use with the standalone application rather than just a configuration (default)",
                    v => exportProject = (v != null) },
                { "list=", "Create lists to back fields if the number of unique values in the field exceed LIST",
                    v => listIf = Int32.Parse(v) },
                { "list-default", "Create a list for every subject's default field",
                    v => listDefault = v != null },
                { "subject-order=", "Give a comma separated list of subjects to explicitly define order within the configuration. Subjects not specified will come after those that were.",
                    v => order = v },
                { "subject-exclude=", "Give a comma separated list of subjects to explicitly exclude from the configuration.",
                    v => exclude = v }
            };

            options.Parse(args);
            var builder = new SqlConnectionStringBuilder(connection);
            if (builder.InitialCatalog == null || builder.DataSource == null)
            {
                Console.Error.WriteLine("No valid connection string given.");
                return;
            }

            // create configuration from database
            var parser = new SqlServerParser(connection);
            var subjectExclude = exclude.Split(',').Select(s => s.Trim()).TakeWhile(s => !String.IsNullOrWhiteSpace(s));
            var config = parser.CreateConfiguration(subjectExclude);
            if (listIf > 0 || listDefault)
                parser.UpdateListFields(config, listIf, listDefault);

            // reorder subjects
            var subjectOrder = new List<string>(order.Split(',').Select(s => s.Trim()).TakeWhile(s => !String.IsNullOrWhiteSpace(s)));
            var newOrder = new List<dbqf.Configuration.ISubject>(config.OrderBy(subject => subject.DisplayName, Comparer<string>.Create((s1, s2) =>
            {
                int i1 = subjectOrder.IndexOf(s1), i2 = subjectOrder.IndexOf(s2);

                if (i1 >= 0 && i2 >= 0)
                    return i1 < i2 ? -1 : 1;
                else if (i1 < 0 && i2 >= 0)
                    return 1;
                else if (i1 >= 0 && i2 < 0)
                    return -1;

                return 0;
            })));
            for (int i = 0; i < config.Count; i++)
                config[i] = newOrder[i];
            
            // create assembler ready for serialization
            var assembler = new dbqf.Serialization.Assemblers.MatrixConfigurationAssembler(
                new dbqf.Serialization.Assemblers.SubjectAssembler(
                    new dbqf.Serialization.Assemblers.FieldAssembler(
                        new dbqf.Serialization.Assemblers.Parsers.ParserAssembler())));

            if (exportProject)
            {
                var projAssembler = new Standalone.Core.Serialization.Assemblers.ProjectAssembler(assembler);

                var project = new Standalone.Core.Project()
                {
                    Id = Guid.NewGuid(),
                    Title = builder.InitialCatalog,
                    Configuration = config,
                    Connections = new List<Standalone.Core.ProjectConnection>()
                    {
                        new Standalone.Core.SqlProjectConnection()
                        {
                            Identifier = "initial",
                            ConnectionString = connection,
                            DisplayName = connection
                        }
                    }
                };

                var dto = projAssembler.Create(project);
                var xml = new XmlSerializer(dto.GetType());
                xml.Serialize(System.Console.Out, dto);
            }
            else
            {
                var dto = assembler.Create(config);
                var xml = new XmlSerializer(dto.GetType());
                xml.Serialize(System.Console.Out, dto);
            }
        }
    }
}
