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
        public WpfAdvancedAdapter(IList<ISubject> subjects, IFieldPathComboBox pathCombo, IParameterBuilderFactory builderFactory, IControlFactory<System.Windows.UIElement> controlFactory)
            : base(subjects, pathCombo, builderFactory, controlFactory)
        {
        }

        public FieldPathComboAdapter FieldPathComboAdapter
        {
            get { return (FieldPathComboAdapter)base._pathCombo; }
        }

        // override required to fire ValueVisibility changed
        public override UIElement<UIElement> UIElement
        {
            get { return base.UIElement; }
            set
            {
                base.UIElement = value;
                OnPropertyChanged("ValueVisibility");
            }
        }

        public Visibility ValueVisibility
        {
            get { return UIElement == null || UIElement.Element == null ? Visibility.Collapsed : Visibility.Visible; }
        }
    }
}
