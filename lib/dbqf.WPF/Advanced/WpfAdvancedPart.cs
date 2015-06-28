using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace dbqf.WPF.Advanced
{
    public abstract class WpfAdvancedPart
    {
        public event EventHandler DeleteRequested = delegate { };
        protected virtual void OnDeleteRequested()
        {
            DeleteRequested(this, EventArgs.Empty);
        }

        public WpfAdvancedPartJunction Container
        {
            get { return _container; }
            set { _container = value; }
        }
        private WpfAdvancedPartJunction _container;

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
    }
}
