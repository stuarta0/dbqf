using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using dbqf.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Standalone.Installers
{
    public class FactoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining<IFieldPathFactory>()
                .InNamespace("dbqf.Display")
                .If(type => type.Name.EndsWith("Factory"))
                .LifestyleSingleton()
                .WithService.DefaultInterfaces());

            container.Register(
                Component.For<IControlFactory<Control>>().UsingFactoryMethod<dbqf.WinForms.UIElements.WinFormsControlFactory>(kernel =>
                {
                    var factory = new dbqf.WinForms.UIElements.WinFormsControlFactory();
                    //factory.ParserLookup = kernel.Resolve<Standalone.Core.Serialization.Assemblers.FieldAssembler>().ParserLookup;
                    return factory;
                }));

            container.Register(Component.For<Standalone.Core.Export.ExportServiceFactory>());
        }
    }
}
