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
using dbqf.Criterion;

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
            // load configuration mapping and data source for chinook
            var config = new dbqf.core.tests.Chinook();
            var source = new dbqf.Hierarchy.Data.SQLiteDataSource(config, @"Data Source=E:\Projects\Programming\dbqf\lib\dbqf.tests\Chinook.sqlite;Version=3;");

            // define the tree we want to see
            var root = new dbqf.Hierarchy.TemplateTreeNode()
                .AddChildren(new dbqf.Hierarchy.SubjectTemplateTreeNode(source)
                    {
                        Subject = config.Artist,
                        Text = "{ArtistId}: {Name}",
                        SearchParameterLevels = 1
                    }.AddOrderBy(
                        new dbqf.OrderedField(new FieldPath(config.Artist["Name"]))
                    ).AddChildren(
                        new dbqf.Hierarchy.SubjectTemplateTreeNode(source)
                        {
                            Subject = config.Album,
                            Text = "{AlbumId}: {Title}",
                            SearchParameterLevels = 1
                        }.AddOrderBy(
                            new dbqf.OrderedField(new FieldPath(config.Album["Title"]))
                        ).AddChildren(
                            new dbqf.Hierarchy.SubjectTemplateTreeNode(source)
                            {
                                Subject = config.Track,
                                Text = "{TrackId}: {GN} - {Name}",
                                SearchParameterLevels = 1
                            }.AddAdditionalField(
                                new FieldPath(config.Track["MTN"])
                            ).AddOrderBy(
                                new dbqf.OrderedField(new FieldPath(config.Track["GN"])),
                                new dbqf.OrderedField(new FieldPath(config.Track["Name"]))
                            )
                        )
                    )
                );
            
            // initialise the root view model
            var rootViewModel = new dbqf.Hierarchy.Display.DataTreeNodeViewModel(root, null, true);
            
            // define the view that we want to see
            var dialog = new Hierarchy.TreeView();
            dialog.SetContext(rootViewModel);
            var view = new dbqf.WPF.StandardView(
                new dbqf.WPF.Standard.WpfStandardAdapter(
                    new dbqf.WPF.UIElements.WpfControlFactory(),
                    new dbqf.Sql.Criterion.ParameterBuilderFactory()));
            view.Adapter.SetPaths(new dbqf.Display.FieldPathFactory().GetFields(config.Track));
            
            // handle the search request to reload the tree
            IParameter where = null;
            view.Adapter.Search += (sender, e) =>
            {
                Console.WriteLine("Search");
                where = view.Adapter.GetParameter();

                // reset tree
                rootViewModel.Reset();
                rootViewModel.IsExpanded = true;
            };
            
            // create styling and modifying the where clause when a search is requested
            ((dbqf.Hierarchy.SubjectTemplateTreeNode)root[0]).DataSourceLoading += (sender, e) =>
            {
                Console.WriteLine("Root.DataSourceLoad fired for {0}", sender);

                // customise our own rendering of each node by binding to a data attribute
                var template = ((dbqf.Hierarchy.SubjectTemplateTreeNode)sender);
                if (template.Subject == config.Track)
                    e.Data.Add("fa-icon", "playcircle");
                else
                    e.Data.Add("fa-icon", "folder");

                // inject additional where clause supplied by user
                // test to see that we can limit non-leaf nodes with criteria, but allow all tracks to load
                // e.g. track name contains "xyz" therefore limit artists and albums, but show all tracks when album expanded
                if (where != null && template.Subject != config.Track)
                    e.Where = e.Where == null ? (dbqf.Sql.Criterion.ISqlParameter)where : new dbqf.Sql.Criterion.SqlConjunction() { e.Where, where };
            };

            // initial load the root children
            rootViewModel.IsExpanded = true;

            // test: try expanding the tree to a specific node
            var walker = new dbqf.Hierarchy.Display.DataTreeNodeWalker(source, rootViewModel);

            //2234, Them And Us - Dark Side Of The Moon, Pink Floyd
            //2193, Once - Ten, Pearl Jam
            var node = walker.ExpandTo((dbqf.Hierarchy.SubjectTemplateTreeNode)root[0][0][0], 2234);
            if (node != null)
                node.IsSelected = true;

            // set context and show the tree and search controls
            dialog.SearchControls.DataContext = view;
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
