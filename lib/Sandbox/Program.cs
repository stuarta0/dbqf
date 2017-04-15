using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Linq;
using dbqf.Configuration;
using dbqf.Serialization.Assemblers;
using dbqf.Serialization.Assemblers.Parsers;
using Standalone.Core;
using Standalone.Core.Serialization.Assemblers;
using Standalone.Core.Serialization.DTO;
using dbqf.Sql.Configuration;

namespace Sandbox
{
    class Program
    {

        private static Type[] GetSerializationTypes()
        {
            return new Type[] { 
                typeof(SqlProjectConnection),
                typeof(SQLiteProjectConnection)
            };
        }

        [STAThread]
        static void Main(string[] args)
        {
            var config = new dbqf.core.tests.Chinook();
            var source = new dbqf.Hierarchy.Data.SQLiteDataSource(config, @"Data Source=E:\Projects\Programming\dbqf\lib\dbqf.tests\Chinook.sqlite;Version=3;");

            var root = (dbqf.Hierarchy.SubjectTemplateTreeNode)new dbqf.Hierarchy.SubjectTemplateTreeNode(source)
            {
                Subject = config.Artist,
                Text = "{ArtistId}: {Name}",
                SearchParameterLevels = 1
            }.AddChildren(
                new dbqf.Hierarchy.SubjectTemplateTreeNode(source)
                {
                    Subject = config.Album,
                    Text = "{AlbumId}: {Title}",
                    SearchParameterLevels = 1
                }.AddChildren(
                    new dbqf.Hierarchy.SubjectTemplateTreeNode(source)
                    {
                        Subject = config.Track,
                        Text = "{TrackId}: {Name}",
                        SearchParameterLevels = 1
                    }
                )
            );
            
            root.DataSourceLoad += (sender, e) =>
            {
                Console.WriteLine("Root.DataSourceLoad fired for {0}", sender);

                // customise our own rendering of each node by binding to a data attribute
                var template = ((dbqf.Hierarchy.SubjectTemplateTreeNode)sender);
                if (template.Subject == config.Artist)
                    e.Data.Add("fa-icon", "user");
                else if (template.Subject == config.Album)
                    e.Data.Add("fa-icon", "square");
                else if (template.Subject == config.Track)
                    e.Data.Add("fa-icon", "playcircle");

                // inject additional where clause by simulating a user search for Album.Title LIKE "moon"
                var where = new dbqf.Sql.Criterion.SqlLikeParameter(
                        config.Album["Title"],
                        "moon",
                        dbqf.Criterion.MatchMode.Anywhere
                    );
                
                e.Where = e.Where == null ? (dbqf.Sql.Criterion.ISqlParameter)where : new dbqf.Sql.Criterion.SqlConjunction() { e.Where, where } ;
            };

            var rootViewModel = new dbqf.Hierarchy.Display.TreeNodeViewModel(null, false);
            foreach (var childNode in root.Load(null))
                rootViewModel.Children.Add(childNode);

            var findId = (object)2234; // track id 2234, Us And Them - Dark Side of the Moon, Pink Floyd
            var target = root[0][0] as dbqf.Hierarchy.SubjectTemplateTreeNode; // track

            // [ artist, album, track ]
            var path = new List<dbqf.Hierarchy.ITemplateTreeNode>();
            var curT = (dbqf.Hierarchy.ITemplateTreeNode)target;
            while (curT != null)
            {
                path.Insert(0, curT);
                curT = curT.Parent;
            }

            // get the id's of all nodes along the path to the target
            var data = source.GetData(target.Subject, 
                path
                    .TakeWhile(t => t is dbqf.Hierarchy.SubjectTemplateTreeNode)
                    .Select<dbqf.Hierarchy.ITemplateTreeNode, dbqf.Criterion.IFieldPath>(t => dbqf.Criterion.FieldPath.FromDefault(((dbqf.Hierarchy.SubjectTemplateTreeNode)t).Subject.IdField))
                    .ToList(), 
                new dbqf.Sql.Criterion.SqlSimpleParameter(
                    target.Subject.IdField, "=", findId));

            // expand each node and find the relevant target it along the way
            var children = rootViewModel.Children;
            int curIdx = 0;
            while (curIdx < path.Count)
            {
                // 3 things need to be synchronised:
                // 1) tree templates
                // 2) data ids
                // 3) tree data

                // 1) tree template
                curT = path[curIdx];

                // 2) data ids 
                // TODO: data ids won't always be in sync with the path (assuming other levels in the tree aren't always SubjectTemplateTreeNodes
                findId = data.Rows[0][curIdx];

                // 3) tree data
                var node = children.Find(vm =>
                {
                    var d = ((dbqf.Hierarchy.Display.DataTreeNodeViewModel)vm);
                    return d.TemplateNode == curT && findId.Equals(d.Data[((dbqf.Hierarchy.SubjectTemplateTreeNode)d.TemplateNode).Subject.IdField.SourceName]);
                });

                // if the node wasn't found within the child collection (filtering may have excluded it) stop
                if (node == null)
                    break;

                // if we're at the end of the search, select the node, otherwise continue expanding
                if (curIdx == path.Count - 1)
                {
                    node.IsSelected = true;
                    break;
                }
                else
                    node.IsExpanded = true;

                curIdx++;
                children = node.Children;
            }


            var dialog = new Hierarchy.TreeView();
            dialog.SetContext(rootViewModel);
            dialog.ShowDialog();

            return;

            //ConfigurationConvertor.Convert(
            //    @"E:\assetasyst.xml",
            //    @"E:\assetasyst.proj.xml");

            var assembler = new ProjectAssembler(new MatrixConfigurationAssembler(new SubjectAssembler(new FieldAssembler(new ParserAssembler()))));
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ProjectDTO), GetSerializationTypes());
            Project p = new Project()
            {
                Configuration = new dbqf.core.tests.Chinook(),
                Connections = new List<ProjectConnection>()
                {
                    new SQLiteProjectConnection() { DisplayName = "sqlite", Identifier = "1", ConnectionString = "abc" },
                    new SqlProjectConnection() { DisplayName = "mssql", Identifier = "2", ConnectionString = "abc" }
                }
            };


            var dto = assembler.Create(p);
            //var list = new List<dbqf.Serialization.DTO.Parsers.ParserDTO>();
            //list.Add(new dbqf.Serialization.DTO.Parsers.DelimitedParserDTO(new string[] { ",", ";", "<", Environment.NewLine, "\"", "\t" }));
            //list.Add(new dbqf.Serialization.DTO.Parsers.ConvertParserDTO() { FromType = typeof(object).FullName, ToType = typeof(string).FullName });
            //dto.Configuration.Subjects[0].Fields[0].Parsers = list;
            //File.Delete(@"E:\chinook.proj.xml");

            var ws = new System.Xml.XmlWriterSettings();
            ws.Indent = true;
            ws.IndentChars = "  ";
            ws.CheckCharacters = true;
            using (XmlWriter writer = XmlWriter.Create(@"E:\chinook.proj.xml", ws))
            //using (var file = File.CreateText(@"E:\chinook.proj.xml"))
            //using (var writer = new dbqf.Serialization.NonXsiTextWriter(file))
            {
                //writer.Settings.CheckCharacters = true;
                //writer.Settings.Indent = true;
                //writer.Settings.IndentChars = "  ";
                serializer.Serialize(writer, dto);
            }

            //File.WriteAllText(@"E:\AssetAsystConfiguration.cs", new FluentGenerator().Generate(new dbqf.core.tests.Chinook(), "dbqf.core.tests", "Chinook"));




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

            File.WriteAllText(@"E:\assetasyst.cs", new FluentGenerator().Generate(p.Configuration, "dbqf.AssetAsyst", "Configuration"));

            var validator = new ConfigurationValidation(p.Configuration, new SqlConnection(((SqlProjectConnection)p.Connections[0]).ConnectionString));
            validator.Validate(true);

            Console.WriteLine("\nDone.");
            Console.ReadKey();
        }

        private static IConfiguration Build()
        {
            ISqlSubject user, module, file;

            return new MatrixConfiguration()
                .Subject(
                    user = new SqlSubject("User")
                        .SqlQuery("SELECT * FROM Users")
                        .FieldId(new Field("Id", typeof(Guid)))
                        .FieldDefault(new Field("Name", typeof(string)) { List = new FieldList() { Source = "SELECT Id, Name AS Value FROM Users", Type = FieldListType.Suggested } })
                        .Field(new Field("RegistrationCode", "Registration Code", typeof(string))))
                .Subject(
                    module = new SqlSubject("Module")
                        .SqlQuery("SELECT * FROM tblModule")
                        .FieldId(new Field("module_id", typeof(int)))
                        .FieldDefault(new Field("module_name", "Name", typeof(string)) { List = new FieldList() { Source = "SELECT module_id Id, module_name AS Value FROM tblModule", Type = FieldListType.Suggested } })
                        .Field(new Field("module_software", "Software", typeof(string)) { List = new FieldList() { Source = "SELECT DISTINCT module_software FROM tblModule", Type = FieldListType.Limited } }))
                .Subject(
                    file = new SqlSubject("File")
                        .SqlQuery("SELECT * FROM tblFile")
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
