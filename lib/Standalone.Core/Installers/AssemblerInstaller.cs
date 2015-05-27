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
                Component.For<AssemblyLine<dbqf.Criterion.IParameter, Standalone.Core.Serialization.DTO.Criterion.ParameterDTO>>().UsingFactoryMethod(kernel => {
                    var pathAssembler = kernel.Resolve<FieldPathAssembler>();
                    ParameterAssembler chain = new NullParameterAssembler(null, pathAssembler);
                    chain = new SimpleParameterAssembler(chain, pathAssembler);
                    chain = new JunctionParameterAssembler(chain);
                    chain = new NotParameterAssembler(chain);
                    return chain;
                }));
        }
    }
}
