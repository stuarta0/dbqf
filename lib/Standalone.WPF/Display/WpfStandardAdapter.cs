using System.ComponentModel;
using System.Linq;
using System.Windows;
using dbqf.Display;
using dbqf.Display.Standard;
using Standalone.Core.Data;

namespace Standalone.WPF.Display
{
    public class WpfStandardAdapter : dbqf.WPF.Standard.WpfStandardAdapter
    {
        public ParserFactory Parser { get; set; }

        public WpfStandardAdapter(IControlFactory<UIElement> controlFactory, IParameterBuilderFactory builderFactory, ParserFactory parserFactory)
            : base(controlFactory, builderFactory)
        {
            Parser = parserFactory;
        }

        protected override dbqf.Display.Standard.StandardPart<UIElement> CreatePart()
        {
            var part = base.CreatePart();
            part.Parser = Parser.Create(part.SelectedPath, part.SelectedBuilder);
            return part;
        }

        protected override void OnPartChanged(StandardPart<UIElement> part, PropertyChangedEventArgs e)
        {
            base.OnPartChanged(part, e);

            if (new [] { "SelectedPath", "SelectedBuilder" }.Contains(e.PropertyName))
                part.Parser = Parser.Create(part.SelectedPath, part.SelectedBuilder);
        }
    }
}
