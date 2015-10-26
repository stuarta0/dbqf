using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using dbqf.Sql.Configuration;
using Standalone.Core;
using Standalone.Core.Data;
using Standalone.WPF.Properties;

namespace Standalone.WPF.Installers
{
    public class DataInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<DbServiceFactory>().UsingFactoryMethod<DbServiceFactory>(kernel =>
                    {
                        return new DbServiceFactory(kernel.Resolve<IMatrixConfiguration>(), Settings.Default.CommandTimeout);
                    }),
                Component.For<ListCacher>()
            );
        }
    }
}
