using System.Windows.Forms;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using dbqf.Display;

namespace Standalone.Installers
{
    public class WinFormsFactoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IControlFactory<Control>>().ImplementedBy<dbqf.WinForms.UIElements.WinFormsControlFactory>());
        }
    }
}
