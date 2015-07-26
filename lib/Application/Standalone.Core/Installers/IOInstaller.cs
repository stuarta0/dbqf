using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Standalone.Core.Export;

namespace Standalone.Core.Installers
{
    public class IOInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IExportServiceFactory>().ImplementedBy<ExportServiceFactory>(),
                Component.For<IViewPersistence>().ImplementedBy<XmlViewPersistence>());
        }
    }
}
