using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using dbqf.Display.Preset;
using dbqf.WinForms;
using dbqf.WinForms.Preset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Standalone.Installers.Views
{
    public class PresetInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Lifestyle Singleton because we know the control will only be used once in our application.
            container.Register(
                Component.For<PresetView>().LifestyleSingleton(),
                Component.For<PresetAdapter<Control>>().ImplementedBy<Standalone.Core.Display.PresetAdapter<Control>>().LifestyleSingleton()
            );
        }
    }
}
