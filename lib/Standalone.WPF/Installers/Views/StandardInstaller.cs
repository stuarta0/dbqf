using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using dbqf.Display.Preset;
using dbqf.Display.Standard;
using dbqf.WPF;
using dbqf.WPF.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.WPF.Installers.Views
{
    public class StandardInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<StandardView>().LifestyleTransient(),
                Component.For<StandardAdapter<System.Windows.UIElement>>().ImplementedBy<WpfStandardAdapter>().LifestyleTransient()
            );
        }
    }
}
