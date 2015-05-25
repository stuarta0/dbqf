using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
