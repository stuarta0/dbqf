using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using dbqf.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Standalone.Forms;

namespace Standalone.Installers
{
    public class ApplicationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<Shell>(),
                Component.For<Main>(),
                Component.For<MainAdapter>(),
                Component.For<RetrieveFieldsView>(),
                Component.For<RetrieveFieldsViewAdapter>(),
                Component.For<Standalone.Core.IApplication>().UsingFactoryMethod<MainAdapter>(kernel => kernel.Resolve<MainAdapter>())
            );
        }
    }
}
