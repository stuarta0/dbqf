using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Display;
using Standalone.Core.Data;
using dbqf.Criterion;

namespace Standalone.Core.Display
{
    public class StandardAdapter<T> : dbqf.Display.Standard.StandardAdapter<T>
    {
        public ParserFactory Parser { get; set; }

        public StandardAdapter(IControlFactory<T> controlFactory, IParameterBuilderFactory builderFactory, ParserFactory parserFactory)
            : base(controlFactory, builderFactory)
        {
            Parser = parserFactory;
        }

        protected override dbqf.Display.Standard.StandardPart<T> CreatePart()
        {
            var part = base.CreatePart();
            part.Parser = Parser.Create(part.SelectedPath, part.SelectedBuilder);
            return part;
        }

        protected override void OnPartChanged(dbqf.Display.Standard.StandardPart<T> part, PropertyChangedEventArgs e)
        {
            base.OnPartChanged(part, e);

            if (new [] { "SelectedPath", "SelectedBuilder" }.Contains(e.PropertyName))
                part.Parser = Parser.Create(part.SelectedPath, part.SelectedBuilder);
        }
    }
}
