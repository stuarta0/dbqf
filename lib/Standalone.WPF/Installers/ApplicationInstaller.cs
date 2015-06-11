using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Standalone.Core;
using Standalone.WPF.ViewModel;

namespace Standalone.WPF.Installers
{
    public class ApplicationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<Shell>(),
                Component.For<IShell>().UsingFactoryMethod<Shell>(kernel => kernel.Resolve<Shell>()),
                Component.For<Standalone.WPF.MainWindow>(),
                Component.For<Standalone.WPF.MainWindowAdapter>(),
                Component.For<Standalone.WPF.Controls.RetrieveFieldsView>(),
                Component.For<Standalone.WPF.Controls.RetrieveFieldsViewAdapter>(),
                Component.For<Standalone.Core.IApplication>().UsingFactoryMethod<MainWindowAdapter>(kernel => kernel.Resolve<MainWindowAdapter>()),
                Component.For<IDialogService>().ImplementedBy<DialogService>()
            );
        }
    }
}
