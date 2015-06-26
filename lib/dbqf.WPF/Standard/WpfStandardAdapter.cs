using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace dbqf.WPF.Standard
{
    public class WpfStandardAdapter : StandardAdapter<UIElement>
    {
        public WpfStandardAdapter(IControlFactory<UIElement> controlFactory, IParameterBuilderFactory builderFactory)
            : base(controlFactory, builderFactory)
        {
        }

        protected override StandardPart<UIElement> CreatePart()
        {
            var part = new WpfStandardPart(_builderFactory, _controlFactory);
            part.Paths = new BindingList<IFieldPath>(_paths);
            return part;
        }

        public ICommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                    _addCommand = new RelayCommand(p => base.AddPart());
                return _addCommand;
            }
        }
        private ICommand _addCommand;
    }
}
