using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Standalone.Core.Serialization.Assemblers;
using Standalone.Core.Serialization.Assemblers.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Standalone.Core.Serialization.DTO.Builders;
using dbqf.Display.Builders;
using dbqf.Criterion;
using Standalone.Core.Serialization.DTO.Criterion;
using Standalone.Core.Serialization.Assemblers.Builders;

namespace Standalone.Core.Installers
{
    public class AssemblerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                .InNamespace("Standalone.Core.Serialization.Assemblers")
                .WithService.DefaultInterfaces());

            container.Register(
                Classes.FromThisAssembly()
                .InNamespace("Standalone.Core.Serialization.Assemblers.Parsers")
                .WithService.DefaultInterfaces());

            container.Register(
                Classes.FromThisAssembly()
                .InNamespace("Standalone.Core.Serialization.Assemblers.Display")
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
