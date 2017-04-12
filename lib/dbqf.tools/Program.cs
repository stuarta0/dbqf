using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDesk.Options;
using System.Xml.Serialization;

namespace dbqf.tools
{
    class Program
    {
        static void Main(string[] args)
        {
            string connection = "";
            var options = new OptionSet() {
                { "connection=", v => connection = v }
            };

            options.Parse(args);
            var config = new SqlServerParser(connection).CreateConfiguration();

            var assembler = new dbqf.Serialization.Assemblers.MatrixConfigurationAssembler(
                new dbqf.Serialization.Assemblers.SubjectAssembler(
                    new dbqf.Serialization.Assemblers.FieldAssembler(
                        new dbqf.Serialization.Assemblers.Parsers.ParserAssembler())));

            var dto = assembler.Create(config);
            var xml = new XmlSerializer(dto.GetType());
            xml.Serialize(System.Console.Out, dto);
        }
    }
}
