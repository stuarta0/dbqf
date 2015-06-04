using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using dbqf.Criterion;
using dbqf.Serialization.Assemblers;
using dbqf.Serialization.Assemblers.Builders;
using dbqf.Serialization.Assemblers.Criterion;
using dbqf.Serialization.DTO.Criterion;
using Standalone.Core.Serialization.Assemblers;

namespace Standalone.Core.Installers
{
    public class AssemblerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ProjectAssembler>(),
                Classes.FromAssemblyContaining<ConfigurationAssembler>()
                    .InNamespace("dbqf.Serialization.Assemblers")
                    .WithService.DefaultInterfaces(),
                Classes.FromAssemblyContaining<ConfigurationAssembler>()
                    .InNamespace("dbqf.Serialization.Assemblers.Parsers")
                    .WithService.DefaultInterfaces(),
                Classes.FromAssemblyContaining<ConfigurationAssembler>()
                    .InNamespace("dbqf.Serialization.Assemblers.Display")
                    .WithService.DefaultInterfaces());

            container.Register(
                Component.For<AssemblyLine<IParameter, ParameterDTO>>().UsingFactoryMethod(kernel => {
                    var pathAssembler = kernel.Resolve<FieldPathAssembler>();
                    var root = new JunctionParameterAssembler();
                    root.Add(new NullParameterAssembler(pathAssembler))
                        .Add(new NotParameterAssembler())
                        .Add(new SimpleParameterAssembler(pathAssembler));
                    return root;
                }));

            container.Register(
                Component.For<BuilderAssembler>().UsingFactoryMethod(kernel =>
                {
                    BuilderAssembler root = new JunctionBuilderAssembler();
                    root.Add(new NullBuilderAssembler())
                        .Add(new NotBuilderAssembler())
                        .Add(new SimpleBuilderAssembler())
                        .Add(new BooleanBuilderAssembler())
                        .Add(new LikeBuilderAssembler())
                        .Add(new BetweenBuilderAssembler());

                    var item = root;
                    while (item != null)
                    {
                        item.Chain = root;
                        item = (BuilderAssembler)item.Successor;
                    }

                    return root;
                }));
        }
    }
}
