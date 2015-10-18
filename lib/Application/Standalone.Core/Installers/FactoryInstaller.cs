using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using dbqf.Criterion;
using dbqf.Display;
using dbqf.Sql.Criterion;

namespace Standalone.Core.Installers
{
    public class FactoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining<IFieldPathFactory>()
                .InNamespace("dbqf.Display")
                .If(type => type.Name.EndsWith("Factory"))
                .LifestyleSingleton()
                .WithService.DefaultInterfaces());

            container.Register(Component.For<IParameterBuilderFactory>()
                .ImplementedBy<ParameterBuilderFactory>().LifestyleSingleton());
        }
    }
}
