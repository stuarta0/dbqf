using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace dbqf.WinForms.Standard
{
    /// <summary>
    /// Exists to allow selection as DataSource on VS form designer.
    /// </summary>
    public class WinFormsStandardPart : StandardPart<Control>
    {
        public WinFormsStandardPart(IParameterBuilderFactory builderFactory, IControlFactory<Control> controlFactory)
            : base(builderFactory, controlFactory)
        {
        }
    }
}
