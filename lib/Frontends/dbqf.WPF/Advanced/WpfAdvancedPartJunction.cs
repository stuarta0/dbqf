using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Advanced;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace dbqf.WPF.Advanced
{
    public class WpfAdvancedPartJunction : AdvancedPartJunction
    {
        public WpfAdvancedPartJunction()
            : base()
        { }
        public WpfAdvancedPartJunction(JunctionType type)
            : base(type)
        { }

        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand(p => OnDeleteRequested());
                return _deleteCommand;
            }
        }
        private ICommand _deleteCommand;

        public ICommand SelectCommand
        {
            get
            {
                if (_selectCommand == null)
                    _selectCommand = new RelayCommand(p => IsSelected = !IsSelected);
                return _selectCommand;
            }
        }
        private ICommand _selectCommand;
    }
}
