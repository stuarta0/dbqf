using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using dbqf.Configuration;
using dbqf.Serialization.Assemblers;
using dbqf.Serialization.Assemblers.Parsers;
using Standalone.Core;
using Standalone.Core.Serialization.Assemblers;
using Standalone.Core.Serialization.DTO;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            //ConfigurationConvertor.Convert(
            //    @"E:\Users\sattenborrow\Documents\Visual Studio 2010\Projects\db-query-framework\configurations\pittsh-helpdesk.xml",
            //    @"E:\Users\sattenborrow\Documents\Visual Studio 2010\Projects\db-query-framework\configurations\pittsh-helpdesk.proj.xml");

            var assembler = new ProjectAssembler(new ConfigurationAssembler(new SubjectAssembler(new FieldAssembler(new ParserAssembler()))));
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ProjectDTO));
            Project p;


            var dto = assembler.Create(new Project() { Configuration = new dbqf.tests.Chinook() });
            var list = new List<dbqf.Serialization.DTO.Parsers.ParserDTO>();
            list.Add(new dbqf.Serialization.DTO.Parsers.DelimitedParserDTO(new string[] { ",", ";", "<", Environment.NewLine, "\"", "\t" }));
            list.Add(new dbqf.Serialization.DTO.Parsers.ConvertParserDTO() { FromType = typeof(object).FullName, ToType = typeof(string).FullName });
            dto.Configuration.Subjects[0].Fields[0].Parsers = list;
            File.Delete(@"E:\chinook.proj.xml");

            var ws = new System.Xml.XmlWriterSettings();
            ws.Indent = true;
            ws.IndentChars = "  ";
            ws.CheckCharacters = true;
            using (XmlWriter writer = XmlWriter.Create(@"E:\chinook.proj.xml", ws))
                serializer.Serialize(writer, dto);

            //p = new Project()
            //{
            //    Id = Guid.NewGuid(),
            //    Configuration = Build(),
            //    Connections = new List<Connection>() 
            //    { 
            //        new Connection() 
            //        { 
            //            ConnectionType = "SqlClient", 
            //            DisplayName = "Live", 
            //            Identifier = "live", 
            //            ConnectionString = "Server=LAU-SQL2005;Database=DownloadCentre;Trusted_Connection=True;" } 
            //    }
            //};
            //var dto = assembler.Create(p);
            //dto.Configuration[0].Fields[1].Parsers.Add(new ParserDTO() { TypeName = typeof(DelimitedParser).Name, Delimiters = new string[] { "," } });
            //using (TextWriter writer = new StreamWriter(@"E:\Users\sattenborrow\Documents\Visual Studio 2010\Projects\db-query-framework\configurations\download-centre.proj.xml"))
            //    serializer.Serialize(writer, dto);


            var rs = new XmlReaderSettings();
            rs.CheckCharacters = true; // required to read special characters like new line and tab
            using (XmlReader reader = XmlReader.Create(@"E:\chinook.proj.xml", rs))
                p = assembler.Restore((ProjectDTO)serializer.Deserialize(reader));

            var validator = new ConfigurationValidation(p.Configuration, new SqlConnection(p.Connections[0].ConnectionString));
            validator.Validate(true);

            Console.WriteLine("\nDone.");
            Console.ReadKey();
        }

        private static IConfiguration Build()
        {
            ISubject user, module, file;

            return new ConfigurationImpl()
                .Subject(
                    user = new Subject("User")
                        .Sql("SELECT * FROM Users")
                        .FieldId(new Field("Id", typeof(Guid)))
                        .FieldDefault(new Field("Name", typeof(string)) { List = new FieldList() { Source = "SELECT Id, Name AS Value FROM Users", Type = FieldListType.Suggested } })
                        .Field(new Field("RegistrationCode", "Registration Code", typeof(string))))
                .Subject(
                    module = new Subject("Module")
                        .Sql("SELECT * FROM tblModule")
                        .FieldId(new Field("module_id", typeof(int)))
                        .FieldDefault(new Field("module_name", "Name", typeof(string)) { List = new FieldList() { Source = "SELECT module_id Id, module_name AS Value FROM tblModule", Type = FieldListType.Suggested } })
                        .Field(new Field("module_software", "Software", typeof(string)) { List = new FieldList() { Source = "SELECT DISTINCT module_software FROM tblModule", Type = FieldListType.Limited } }))
                .Subject(
                    file = new Subject("File")
                        .Sql("SELECT * FROM tblFile")
                        .FieldId(new Field("File_id", typeof(int)))
                        .Field(new RelationField("File_Module", "Module", module))
                        .FieldDefault(new Field("File_Description", "Description", typeof(string)))
                        .Field(new Field("File_Type", "Type", typeof(string)) { List = new FieldList() { Source = "SELECT DISTINCT File_Type FROM tblFile", Type = FieldListType.Limited } })
                        .Field(new Field("File_filename", "Filename", typeof(string)))
                        .Field(new Field("File_Version", "Version", typeof(string)))
                        .Field(new Field("File_Comment", "Comment", typeof(string)))
                        .Field(new Field("File_NotesURL", "Notes URL", typeof(string)))
                        .Field(new Field("File_EULA", "EULA URL", typeof(string))))

                .Matrix(user, module,
                    "SELECT u.Id FromId, am.Allowed_Module ToId FROM Users u INNER JOIN UserProducts up ON u.Id = up.UserId INNER JOIN tblAllowedModules am ON up.OldCouncilId = am.Allowed_Council",
                    "Search for modules that are assigned to a user")
                .Matrix(user, file,
                    "SELECT u.Id FromId, f.File_Id ToId FROM Users u INNER JOIN UserProducts up ON u.Id = up.UserId INNER JOIN tblAllowedModules am ON up.OldCouncilId = am.Allowed_Council INNER JOIN tblFile f ON f.File_Module = am.Allowed_Module",
                    "Search for files that are assigned to a user")
                .Matrix(module, user,
                    "SELECT u.Id FromId, am.Allowed_Module ToId FROM Users u INNER JOIN UserProducts up ON u.Id = up.UserId INNER JOIN tblAllowedModules am ON up.OldCouncilId = am.Allowed_Council",
                    "Search for users that are assigned to a module")
                .Matrix(module, file,
                    "SELECT File_Module FromId, File_Id ToId FROM tblFile",
                    "Search for files that are part of a module")
                .Matrix(file, user,
                    "SELECT f.File_Id FromId, u.Id ToId FROM Users u INNER JOIN UserProducts up ON u.Id = up.UserId INNER JOIN tblAllowedModules am ON up.OldCouncilId = am.Allowed_Council INNER JOIN tblFile f ON f.File_Module = am.Allowed_Module",
                    "Search for users that are assigned to a file")
                .Matrix(file, module,
                    "SELECT File_Id FromId, File_Module ToId FROM tblFile",
                    "Search for modules that contain a file");
        }
    }
}
