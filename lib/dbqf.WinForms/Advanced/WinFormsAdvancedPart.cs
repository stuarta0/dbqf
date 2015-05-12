using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Advanced;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace dbqf.WinForms.Advanced
{
    public class WinFormsAdvancedPart : AdvancedPart<Control>
    {
        public WinFormsAdvancedPart(FieldPathFactory pathFactory, ParameterBuilderFactory builderFactory, IControlFactory<Control> controlFactory)
            : base(pathFactory, builderFactory, controlFactory)
        {
        }
    }
}
