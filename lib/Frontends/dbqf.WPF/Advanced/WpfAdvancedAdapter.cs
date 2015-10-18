using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

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

        public ICommand AndCommand
        {
            get
            {
                if (_andCommand == null)
                    _andCommand = new RelayCommand(p => Add(JunctionType.Conjunction));
                return _andCommand;
            }
        }
        private ICommand _andCommand;

        public ICommand OrCommand
        {
            get
            {
                if (_orCommand == null)
                    _orCommand = new RelayCommand(p => Add(JunctionType.Disjunction));
                return _orCommand;
            }
        }
        private ICommand _orCommand;

        protected override AdvancedPartNode CreateNode()
        {
            return new WpfAdvancedPartNode()
            {
                SelectedPath = _pathCombo.SelectedPath,
                SelectedBuilder = SelectedBuilder,
                Values = (UIElement != null ? UIElement.GetValues() : null)
            };
        }

        protected override AdvancedPartJunction CreateJunction(JunctionType type)
        {
            return new WpfAdvancedPartJunction(type);
        }
    }
}
