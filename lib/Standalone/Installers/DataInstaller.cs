using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using dbqf.Configuration;
using Standalone.Core.Data;
using Standalone.Core.Serialization.Assemblers;
using Standalone.Core.Serialization.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Standalone.Core;
using Standalone.Properties;

namespace Standalone.Installers
{
    public class DataInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ResultFactory>().UsingFactoryMethod<ResultFactory>(kernel =>
                    {
                        return new ResultFactory(Settings.Default.CommandTimeout);
                    }),
                Component.For<ListCacher>()
            );
        }
    }
}
