using System;
using Gtk;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System.IO;

namespace Standalone.GtkSharp
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			var container = BootstrapContainer();
			var shell = container.Resolve<Standalone.Core.IShell>();
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
