using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using dbqf.Sql.Configuration;
using Standalone.Core;
using Standalone.Core.Data;

namespace Standalone.Core.Installers
{
    public class DataInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<DbServiceFactory>().UsingFactoryMethod<DbServiceFactory>(kernel =>
                    {
                        return new DbServiceFactory(kernel.Resolve<IMatrixConfiguration>());
                    }),
                Component.For<ListCacher>()
            );
        }
    }
}
