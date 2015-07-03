using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace dbqf.Display.Advanced
{
    public abstract class AdvancedPart : IPartView, INotifyPropertyChanged
    {
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
                if (_isSelected == value)
                    return;
                _isSelected = value;
                OnIsSelectedChanged();
                OnPropertyChanged("IsSelected");
            }
        }
        private bool _isSelected;

        public event EventHandler DeleteRequested = delegate { };
        protected virtual void OnDeleteRequested()
        {
            DeleteRequested(this, EventArgs.Empty);
        }

        public virtual AdvancedPartJunction Container
        {
            get { return _container; }
            set
            {
                if (_container == value)
                    return;
                _container = value;
                OnPropertyChanged("Container");
                OnPropertyChanged("Prefix");
            }
        }
        private AdvancedPartJunction _container;

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
