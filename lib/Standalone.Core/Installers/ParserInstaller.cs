using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using dbqf.Serialization.Assemblers;
using Standalone.Core.Data;

namespace Standalone.Core.Installers
{
    public class ParserInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ParserFactory>().UsingFactoryMethod<ParserFactory>(kernel =>
                {
                    var factory = new ParserFactory();
                    factory.ParserLookup = kernel.Resolve<FieldAssembler>().ParserLookup;
                    return factory;
                }));
        }
    }
}
