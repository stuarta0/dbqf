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
    public class WinFormsStandardAdapter : StandardAdapter<Control>
    {
        public WinFormsStandardAdapter(IControlFactory<Control> controlFactory, IParameterBuilderFactory builderFactory)
            : base(controlFactory, builderFactory)
        {
        }

        protected override StandardPart<Control> CreatePart()
        {
            var part = new WinFormsStandardPart(_builderFactory, _controlFactory);
            part.Paths = new BindingList<IFieldPath>(_paths);
            part.UIElement = _controlFactory.Build(part.Paths[0], _builderFactory.GetDefault(part.Paths[0]));
            return part;
        }
    }
}
