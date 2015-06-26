using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Display;
using Standalone.Core.Data;

namespace Standalone.Core.Display
{
    public class PresetAdapter<T> : dbqf.Display.Preset.PresetAdapter<T>
    {
        public ParserFactory Parser { get; set; }

        public PresetAdapter(IControlFactory<T> controlFactory, IParameterBuilderFactory builderFactory, ParserFactory parserFactory)
            : base(controlFactory, builderFactory)
        {
            Parser = parserFactory;
        }

        protected override dbqf.Display.Preset.PresetPart<T> CreatePart(dbqf.Criterion.IFieldPath path)
        {
            var part = base.CreatePart(path);
            part.Parser = Parser.Create(part.SelectedPath, part.SelectedBuilder);
            return part;
        }
    }
}
