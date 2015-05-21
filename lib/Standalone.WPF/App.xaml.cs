using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Castle.Windsor;
using Castle.Windsor.Installer;

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
