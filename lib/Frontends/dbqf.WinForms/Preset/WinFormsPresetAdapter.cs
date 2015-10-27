using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Preset;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace dbqf.WinForms.Preset
{
    /// <summary>
    /// Exists to allow selection as DataSource on VS form designer.
    /// </summary>
    public class WinFormsPresetAdapter : PresetAdapter<Control>
    {
        public WinFormsPresetAdapter(IControlFactory<Control> controlFactory, IParameterBuilderFactory builderFactory)
            : base(controlFactory, builderFactory)
        {
        }
    }
}
