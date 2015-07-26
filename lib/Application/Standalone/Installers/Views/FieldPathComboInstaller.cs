using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using dbqf.Display;
using dbqf.Display.Advanced;
using dbqf.WinForms;
using dbqf.WinForms.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Standalone.Installers.Views
{
    public class FieldPathComboInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Lifestyle Singleton because we know the control will only be used once in our application.
            container.Register(
                Component.For<FieldPathCombo>().LifestyleSingleton(),
                Component.For<IFieldPathComboBox, FieldPathComboAdapter>().LifestyleSingleton()
            );
        }
    }
}
