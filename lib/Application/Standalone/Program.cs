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
using dbqf.Serialization;

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
                Settings.Default.SavedConnections = new ConnectionDictionary();

            IWindsorContainer container = null;
            Core.IShell shell = null;
            try
            {
                container = BootstrapContainer();
                shell = container.Resolve<Standalone.Core.IShell>();
            }
            catch (Exception ex)
            {
                var message = String.Concat("Unable to initialise the application.\n\n", ex.Message, "\n\nThe stack trace has been copied to your clipboard.");
                MessageBox.Show(message, "dbqf", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Clipboard.SetText(String.Concat(ex.Message, ex.StackTrace), TextDataFormat.UnicodeText);
                Environment.Exit(-1);
            }

            shell.Run();
            container.Dispose();
        }

        private static IWindsorContainer BootstrapContainer()
        {
            return new WindsorContainer().Install(
                Configuration.FromXmlFile(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "castle.config"))
            );
        }
    }
}
