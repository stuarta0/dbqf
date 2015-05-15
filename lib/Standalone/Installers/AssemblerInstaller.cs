using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Standalone.Serialization.Assemblers;
using Standalone.Serialization.Assemblers.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Installers
{
    public class AssemblerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                .InNamespace("Standalone.Serialization.Assemblers")
                .WithService.DefaultInterfaces());

            container.Register(
                Component.For<TransformHandler>().UsingFactoryMethod<TransformHandler>(kernel => {
                    var pathAssembler = kernel.Resolve<FieldPathAssembler>();
                    TransformHandler chain = new NullParameterHandler(null, pathAssembler);
                    chain = new SimpleParameterHandler(chain, pathAssembler);
                    chain = new ConjunctionParameterHandler(chain);
                    chain = new NotParameterHandler(chain);
                    return chain;
                }));
        }
    }
}
