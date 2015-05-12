using dbqf.Configuration;
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
    public class WinFormsAdvancedAdapter : AdvancedAdapter<Control>
    {
        public WinFormsAdvancedAdapter(IList<ISubject> subjects, IFieldPathFactory pathFactory, IParameterBuilderFactory builderFactory, IControlFactory<Control> controlFactory)
            : base(subjects, pathFactory, controlFactory, builderFactory)
        {
        }
    }
}
