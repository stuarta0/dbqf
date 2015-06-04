using System.Windows.Forms;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using dbqf.Display;
using dbqf.Serialization.Assemblers;
using Standalone.Core.Data;
using Standalone.Core.Export;

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
                Component.For<IControlFactory<Control>>().ImplementedBy<dbqf.WinForms.UIElements.WinFormsControlFactory>());

            container.Register(
                Component.For<ParserFactory>().UsingFactoryMethod<ParserFactory>(kernel =>
                {
                    var factory = new ParserFactory();
                    factory.ParserLookup = kernel.Resolve<FieldAssembler>().ParserLookup;
                    return factory;
                }));

            container.Register(
                Component.For<ExportServiceFactory>(),
                Component.For<IViewPersistence>().ImplementedBy<XmlViewPersistence>());
        }
    }
}
