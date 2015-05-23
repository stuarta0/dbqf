using dbqf.Display;
using dbqf.Display.Preset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace dbqf.WPF.Preset
{
    public class WpfPresetAdapter : PresetAdapter<UIElement>
    {
        public WpfPresetAdapter(IControlFactory<UIElement> controlFactory, IParameterBuilderFactory builderFactory)
            : base(controlFactory, builderFactory)
        {
        }
    }
}
