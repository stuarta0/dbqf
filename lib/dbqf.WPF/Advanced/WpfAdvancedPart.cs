using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace dbqf.WPF.Advanced
{
    public abstract class WpfAdvancedPart : INotifyPropertyChanged
    {
        public event EventHandler DeleteRequested = delegate { };
        protected virtual void OnDeleteRequested()
        {
            DeleteRequested(this, EventArgs.Empty);
        }

        public virtual WpfAdvancedPartJunction Container
        {
            get { return _container; }
            set 
            { 
                _container = value;
                OnPropertyChanged("Container");
            }
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
