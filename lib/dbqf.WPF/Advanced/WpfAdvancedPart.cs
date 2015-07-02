using dbqf.Display;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace dbqf.WPF.Advanced
{
    public abstract class WpfAdvancedPart : IPartView, INotifyPropertyChanged
    {
        #region Delete

        public event EventHandler DeleteRequested = delegate { };
        protected virtual void OnDeleteRequested()
        {
            DeleteRequested(this, EventArgs.Empty);
        }
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

        #endregion

        #region Select

        public event EventHandler IsSelectedChanged = delegate { };
        protected virtual void OnIsSelectedChanged()
        {
            IsSelectedChanged(this, EventArgs.Empty);
        }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnIsSelectedChanged();
                OnPropertyChanged("IsSelected");
            }
        }
        private bool _isSelected;

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

        #endregion

        public virtual WpfAdvancedPartJunction Container
        {
            get { return _container; }
            set 
            { 
                _container = value;
                OnPropertyChanged("Container");
                OnPropertyChanged("Prefix");
            }
        }
        private WpfAdvancedPartJunction _container;

        public string Prefix
        {
            get
            {
                if (Container != null && Container.Parts.IndexOf(this) > 0)
                    return Container.TypeName;
                return string.Empty;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region IPartView

        public virtual void CopyFrom(IPartView other)
        {
        }

        public virtual Criterion.IParameter GetParameter()
        {
            return null;
        }

        public virtual bool Equals(IPartView other)
        {
            return false;
        }

        #endregion
    }
}
