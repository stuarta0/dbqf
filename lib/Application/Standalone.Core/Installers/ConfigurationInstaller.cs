using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using dbqf.Configuration;
using dbqf.Sql.Configuration;
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
        public virtual void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<Project>().UsingFactoryMethod<Project>(CreateProject),
                Component.For<IConfiguration, IMatrixConfiguration>()
                    .UsingFactoryMethod<IConfiguration>(kernel => kernel.Resolve<Project>().Configuration),
                Component.For<IList<ISubject>>()
                    .UsingFactoryMethod(kernel => kernel.Resolve<IConfiguration>())
            );
        }

        protected virtual Project CreateProject(IKernel kernel)
        {
            var args = Environment.GetCommandLineArgs();
            var assembler = kernel.Resolve<ProjectAssembler>();
            var deserializer = new System.Xml.Serialization.XmlSerializer(typeof(ProjectDTO), GetSerializationTypes());
            ProjectDTO dto = null;

            try
            {
                using (TextReader reader = new StreamReader(args[1]))
                    dto = (ProjectDTO)deserializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                // halt application (not currently in a message loop)
                // TODO: need to inform the user that we couldn't load the project: this is why
                // we shouldn't have this logic in the installer, we need to farm it out to something
                // else (plus we'll get a more responsive application as it'll load the UI immediately)
                Environment.Exit(-1);
            }

            return assembler.Restore(dto);
        }

        protected virtual Type[] GetSerializationTypes()
        {
            return new Type[] { 
                typeof(SqlProjectConnection),
                typeof(SQLiteProjectConnection)
            };
        }
    }
}
