using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Standalone.Core;
using System;

namespace Standalone.GtkSharp.Installers
{
	public class ApplicationInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
            container.Register(
                Component.For<IShell>().ImplementedBy<Shell>(),
                Component.For<MainWindow>(),
                Component.For<IApplication, MainWindowAdapter>().ImplementedBy<MainWindowAdapter>()
				//Component.For<RetrieveFieldsView>(),
				//Component.For<RetrieveFieldsViewAdapter>()
			);
		}
	}
}

