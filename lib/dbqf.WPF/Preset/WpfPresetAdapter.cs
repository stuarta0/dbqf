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

        protected override PresetPart<UIElement> CreatePart(Criterion.FieldPath path)
        {
        //    var part = new PresetPart<T>(path);
        //    part.Builder = _builderFactory.GetDefault(part.Path);
        //    part.UIElement = _controlFactory.Build(part.Path, part.Builder);
        //    return part;
            
            // create WpfPresetPart which has GridRow and GridColumn properties for binding
            return base.CreatePart(path);
        }
    }
}
