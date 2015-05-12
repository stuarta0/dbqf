using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using dbqf.Configuration;
using dbqf.WinForms;
using Standalone.Data;
using Standalone.Properties;
using Standalone.Serialization.Assemblers;
using Standalone.Serialization.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Installers
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
                    
                    var project = assembler.Restore(dto);
                    var connectionLookup = Settings.Default.SavedConnections;
                    if (connectionLookup.ContainsKey(dto.Id))
                        project.CurrentConnection = project.Connections.Find(c => c.Identifier == connectionLookup[dto.Id]);
                    return project;
                }),
                Component.For<IConfiguration>()
                    .UsingFactoryMethod<IConfiguration>(kernel => kernel.Resolve<Project>().Configuration),
                Component.For<IList<ISubject>>()
                    .UsingFactoryMethod(kernel => kernel.Resolve<IConfiguration>()),
                Component.For<ResultFactory>()
            );
        }
    }
}
