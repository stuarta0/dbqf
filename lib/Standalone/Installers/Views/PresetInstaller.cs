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
            container.Register(
                Component.For<PresetView>().LifestyleTransient(),
                Component.For<PresetAdapter<Control>>().ImplementedBy<WinFormsPresetAdapter>().LifestyleTransient()
            );
        }
    }
}
