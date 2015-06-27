using dbqf.Configuration;
using dbqf.Display;
using dbqf.Display.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace dbqf.WPF.Advanced
{
    public class WpfAdvancedAdapter : AdvancedAdapter<System.Windows.UIElement>
    {
        public WpfAdvancedAdapter(IList<ISubject> subjects, IFieldPathFactory pathFactory, IControlFactory<System.Windows.UIElement> controlFactory, IParameterBuilderFactory builderFactory, IFieldPathComboBox pathCombo)
            : base(subjects, pathFactory, controlFactory, builderFactory, pathCombo)
        {
            
        }

        public FieldPathComboAdapter FieldPathComboAdapter
        {
            get { return (FieldPathComboAdapter)base._pathCombo; }
        }
    }
}
