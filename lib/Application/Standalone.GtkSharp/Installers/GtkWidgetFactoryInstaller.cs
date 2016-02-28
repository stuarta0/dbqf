using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using dbqf.Display;

namespace Standalone.GtkSharp.Installers
{
	public class GtkWidgetFactoryInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				Component.For<IControlFactory<Gtk.Widget>>().ImplementedBy<dbqf.GtkSharp.UIElements.GtkWidgetControlFactory>());
		}
	}
}
