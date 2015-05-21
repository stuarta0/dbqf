using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.WPF.Installers
{
    public class ApplicationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<Shell>(),
                Component.For<Standalone.WPF.MainWindow>()
                //Component.For<Standalone.WPF.MainAdapter>()
            );
        }
    }
}
