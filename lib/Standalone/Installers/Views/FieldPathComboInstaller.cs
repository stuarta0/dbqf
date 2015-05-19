using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
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
            container.Register(
                Component.For<FieldPathCombo>().LifestyleTransient(),
                Component.For<FieldPathComboAdapter>().LifestyleTransient()
            );
        }
    }
}
