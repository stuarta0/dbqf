using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using NDesk.Options;
using Standalone.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Standalone
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Settings.Default.UpgradeRequired)
            {
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;
                Settings.Default.Save();
            }
            if (Settings.Default.SavedConnections == null)
                Settings.Default.SavedConnections = new Serialization.ConnectionDictionary();

            var container = BootstrapContainer();
            var shell = container.Resolve<Shell>();

            //var ass = container.Resolve<Standalone.Serialization.Assemblers.ProjectAssembler>();
            //var prj = new Standalone.Data.Project();
            //prj.Configuration = new dbqf.Tests.AdventureWorks();
            //prj.Connections.Add(new Data.Connection() { DisplayName = "Local", ConnectionType = "SqlClient", Identifier = "mssql", ConnectionString = "" });
            //prj.Id = Guid.NewGuid();
            //var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Standalone.Serialization.DTO.ProjectDTO));
            //using (TextWriter writer = new StreamWriter(@"C:\training\assetasyst\code\db-query-framework\configurations\adventure-works.proj.xml"))
            //{
            //    serializer.Serialize(writer, ass.Create(prj));
            //}
            //return;

            shell.Run();
            container.Dispose();
        }

        private static IWindsorContainer BootstrapContainer()
        {
            return new WindsorContainer()
                .Install(//Configuration.FromAppConfig(),
                         FromAssembly.This()
                );
        }
    }
}
