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

namespace Standalone.Core.Installers
{
    public class ConfigurationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<Project>().UsingFactoryMethod<Project>(kernel => {
                    var args = Environment.GetCommandLineArgs();
                    var assembler = kernel.Resolve<ProjectAssembler>();
                    var deserializer = new System.Xml.Serialization.XmlSerializer(typeof(ProjectDTO));
                    ProjectDTO dto;
                    using (TextReader reader = new StreamReader(args[1]))
                        dto = (ProjectDTO)deserializer.Deserialize(reader);
                    return assembler.Restore(dto);
                }),
                Component.For<IConfiguration>()
                    .UsingFactoryMethod<IConfiguration>(kernel => kernel.Resolve<Project>().Configuration),
                Component.For<IList<ISubject>>()
                    .UsingFactoryMethod(kernel => kernel.Resolve<IConfiguration>())
            );
        }
    }
}
