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
                Settings.Default.SavedConnections = new Core.Serialization.ConnectionDictionary();

            var container = BootstrapContainer();
            var shell = container.Resolve<Shell>();
            shell.Run();
            container.Dispose();
        }

        private static IWindsorContainer BootstrapContainer()
        {
            return new WindsorContainer()
                .Install(//Configuration.FromAppConfig(),
                        FromAssembly.Containing<Standalone.Core.Serialization.DTO.ConfigurationDTO>(),
                        FromAssembly.This()
                );
        }
    }
}
