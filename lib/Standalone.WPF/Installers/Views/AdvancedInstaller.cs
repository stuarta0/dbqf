using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using dbqf.Display.Advanced;
using dbqf.WPF;
using dbqf.WPF.Advanced;

namespace Standalone.WPF.Installers.Views
{
    public class AdvancedInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<AdvancedView>().LifestyleSingleton(),
                Component.For<AdvancedAdapter<System.Windows.UIElement>, WpfAdvancedAdapter>().ImplementedBy<WpfAdvancedAdapter>().LifestyleSingleton()
            );
        }
    }
}
