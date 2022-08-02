using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Standalone.Core.Initialisers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Core.Installers
{
    public class InitialiserInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromAssemblyContaining<InitialiserInstaller>().BasedOn<IInitialiser>().WithService.FromInterface(),
                Component.For<IList<IInitialiser>>().UsingFactoryMethod<List<IInitialiser>>(
                    c => new List<IInitialiser>(c.ResolveAll<IInitialiser>())));
        }
    }
}
