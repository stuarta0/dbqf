using System;
using Gtk;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System.IO;

namespace Standalone.GtkSharp
{
	class MainClass
	{
        [STAThread]
        public static void Main (string[] args)
		{
            //Glib.Thread.Init();
            Gdk.Threads.Init();
            Application.Init ();
            Gdk.Threads.Enter();

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
