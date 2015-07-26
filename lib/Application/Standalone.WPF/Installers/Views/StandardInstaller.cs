using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using dbqf.Display;
using dbqf.Display.Preset;
using dbqf.Display.Standard;
using dbqf.WPF;
using dbqf.WPF.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.WPF.Installers.Views
{
    public class StandardInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<StandardView>().LifestyleSingleton(),
                Component.For<StandardAdapter<System.Windows.UIElement>>().UsingFactoryMethod<Standalone.WPF.Display.WpfStandardAdapter>(kernel => 
                    {
                        var adapter = new Standalone.WPF.Display.WpfStandardAdapter(
                            kernel.Resolve<IControlFactory<System.Windows.UIElement>>(), 
                            kernel.Resolve<IParameterBuilderFactory>(),
                            kernel.Resolve<Core.Data.ParserFactory>());
                        adapter.IsSharedSizeScope = Standalone.WPF.Properties.Settings.Default.StandardSharedSizeScope;
                        return adapter;
                    }).LifestyleSingleton()
            );
        }
    }
}
