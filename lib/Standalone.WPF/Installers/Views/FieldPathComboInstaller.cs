using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using dbqf.Display.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.WPF;

namespace Standalone.WPF.Installers.Views
{
    public class FieldPathComboInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<FieldPathCombo>().LifestyleTransient(),
                Component.For<FieldPathComboAdapter>().LifestyleTransient()
            );
        }
    }
}
