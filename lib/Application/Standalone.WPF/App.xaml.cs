using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Standalone.WPF.Properties;

namespace Standalone.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (Settings.Default.UpgradeRequired)
            {
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;
                Settings.Default.Save();
            }
            if (Settings.Default.SavedConnections == null)
                Settings.Default.SavedConnections = new dbqf.Serialization.ConnectionDictionary();

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
                MessageBox.Show(message, "dbqf", MessageBoxButton.OK, MessageBoxImage.Error);
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
