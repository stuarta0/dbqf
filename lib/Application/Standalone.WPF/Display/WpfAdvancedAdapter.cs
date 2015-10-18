using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Advanced;
using dbqf.WPF.UIElements;
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
            if (part != null)
            {
                part.Parser = Parser.Create(part.SelectedPath, part.SelectedBuilder);

                // if the UIElement was a ListBox and we have null or empty strings, remove them from values
                // if the result is such that the values are empty, return null
                // (we do this here because the Standalone application is inserting an empty item into the list)
                if (UIElement is ListBoxElement)
                {
                    var values = new List<object>();
                    foreach (var v in UIElement.GetValues())
                        if (v != null && !string.Empty.Equals(v))
                            values.Add(v);

                    if (values.Count == 0)
                        return null;
                    part.Values = values.ToArray();
                }
            }
            return part;
        }
    }
}
