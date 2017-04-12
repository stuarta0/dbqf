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
            var options = new OptionSet() {
                { "connection=", "Connection to an SQL Server database that will be used to generate a configuration",
                    v => connection = v },
                { "as-project", "Output a project object for use with the standalone application rather than just a configuration (default)",
                    v => exportProject = (v != null) },
                { "list=", "Create lists to back fields if the number of unique values in the field exceed LIST",
                    v => listIf = Int32.Parse(v) }
            };

            options.Parse(args);
            var builder = new SqlConnectionStringBuilder(connection);
            if (builder.InitialCatalog == null || builder.DataSource == null)
            {
                Console.Error.WriteLine("No valid connection string given.");
                return;
            }

            var config = new SqlServerParser(connection).CreateConfiguration();
            if (listIf > 0)
            {
                // calculate whether a field should have a list backing it
            }
            
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
