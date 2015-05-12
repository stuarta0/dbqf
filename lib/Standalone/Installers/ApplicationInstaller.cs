using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using dbqf.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Installers
{
    public class ApplicationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<Shell>(),
                Component.For<Standalone.Forms.Main>(),
                Component.For<Standalone.Forms.MainAdapter>(),
                Component.For<Standalone.Forms.RetrieveFieldsView>(),
                Component.For<Standalone.Forms.RetrieveFieldsViewAdapter>()
            );
        }
    }
}
