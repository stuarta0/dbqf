using dbqf.Configuration;
using dbqf.Display;
using dbqf.Display.Advanced;
using Standalone.Core.Data;
using System.Collections.Generic;

namespace Standalone.WPF.Display
{
    public class WpfAdvancedAdapter : dbqf.WPF.Advanced.WpfAdvancedAdapter
    {
        public ParserFactory Parser { get; private set; }

        public WpfAdvancedAdapter(IList<ISubject> subjects, IFieldPathComboBox pathCombo, IParameterBuilderFactory builderFactory, IControlFactory<System.Windows.UIElement> controlFactory, ParserFactory parserFactory)
            : base(subjects, pathCombo, builderFactory, controlFactory)
        {
            Parser = parserFactory;
        }

        protected override AdvancedPartNode CreateNode()
        {
            var part = base.CreateNode();
            part.Parser = Parser.Create(part.SelectedPath, part.SelectedBuilder);
            return part;
        }
    }
}
