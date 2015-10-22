using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using dbqf.Criterion;
using dbqf.Criterion.Builders;
using dbqf.Serialization.Assemblers;
using dbqf.Serialization.Assemblers.Builders;
using dbqf.Serialization.Assemblers.Criterion;
using dbqf.Serialization.DTO.Builders;
using dbqf.Serialization.DTO.Criterion;
using dbqf.Sql.Criterion.Builders;
using Standalone.Core.Serialization.Assemblers;

namespace Standalone.Core.Installers
{
    public class AssemblerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ProjectAssembler>(),
                Classes.FromAssemblyContaining<MatrixConfigurationAssembler>()
                    .InNamespace("dbqf.Serialization.Assemblers")
                    .WithService.DefaultInterfaces(),
                Classes.FromAssemblyContaining<MatrixConfigurationAssembler>()
                    .InNamespace("dbqf.Serialization.Assemblers.Parsers")
                    .WithService.DefaultInterfaces(),
                Classes.FromAssemblyContaining<MatrixConfigurationAssembler>()
                    .InNamespace("dbqf.Serialization.Assemblers.Display")
                    .WithService.DefaultInterfaces());

            container.Register(
                Component.For<SearchDocumentAssembler>());

            container.Register(
                Component.For<AssemblyLine<IParameter, ParameterDTO>>().UsingFactoryMethod(kernel => {
                    var pathAssembler = kernel.Resolve<FieldPathAssembler>();
                    var root = new JunctionParameterAssembler(kernel.Resolve<IParameterBuilderFactory>());
                    root.Add(new NullParameterAssembler(pathAssembler))
                        .Add(new NotParameterAssembler())
                        .Add(new SimpleParameterAssembler(pathAssembler));
                    return root;
                }));

            container.Register(
                Component.For<BuilderAssembler>().UsingFactoryMethod(kernel =>
                {
                    BuilderAssembler root = new JunctionBuilderAssembler();
                    root.Add(new NotBuilderAssembler())
                        .Add(new SimpleBuilderAssembler())
                        .Add(new BooleanBuilderAssembler())
                        .Add(new LikeBuilderAssembler())
                        .Add(new BuilderAssembler<BetweenBuilder, BetweenBuilderDTO>())
                        .Add(new BuilderAssembler<NullBuilder, NullBuilderDTO>())
                        .Add(new BuilderAssembler<DateBetweenBuilder, DateBetweenBuilderDTO>())
                        .Add(new BuilderAssembler<DateEqualsBuilder, DateEqualsBuilderDTO>())
                        .Add(new BuilderAssembler<DateGtBuilder, DateGtBuilderDTO>())
                        .Add(new BuilderAssembler<DateGtEqualBuilder, DateGtEqualBuilderDTO>())
                        .Add(new BuilderAssembler<DateLtBuilder, DateLtBuilderDTO>())
                        .Add(new BuilderAssembler<DateLtEqualBuilder, DateLtEqualBuilderDTO>());

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
