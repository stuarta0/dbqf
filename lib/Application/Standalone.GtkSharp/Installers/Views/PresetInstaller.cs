using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using dbqf.GtkSharp;
using dbqf.Display.Preset;

namespace Standalone.GtkSharp.Installers.Views
{
    public class PresetInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Lifestyle Singleton because we know the control will only be used once in our application.
            container.Register(
                Component.For<PresetView>().LifestyleSingleton(),
				Component.For<PresetAdapter<Gtk.Widget>>().ImplementedBy<Standalone.Core.Display.PresetAdapter<Gtk.Widget>>().LifestyleSingleton()
            );
        }
    }
}
