using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace dbqf.WPF.Standard
{
    /// <summary>
    /// Exists to allow selection as DataSource on VS form designer.
    /// </summary>
    public class WpfStandardPart : StandardPart<UIElement>
    {
        public WpfStandardPart(IParameterBuilderFactory builderFactory, IControlFactory<UIElement> controlFactory)
            : base(builderFactory, controlFactory)
        {
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand(p => base.Remove());
                return _deleteCommand;
            }
        }
        private ICommand _deleteCommand;
    }
}
