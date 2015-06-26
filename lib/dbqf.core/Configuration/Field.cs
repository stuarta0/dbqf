using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace dbqf.Configuration
{
    /// <summary>
    /// A field is essentially a column in a database table but allows additional functionality like obtaining a list of possible values for the field.
    /// </summary>
    public class Field : IField, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the parent subject that contains this field.
        /// </summary>
        public virtual ISubject Subject
        {
            get { return _parent; }
            set
            {
                if (_parent == value)
                    return;

                _parent = value;
                OnPropertyChanged("Subject");
            }
        }
        private ISubject _parent;

        /// <summary>
        /// Gets or sets the source name of this field in the Subject's data.
        /// </summary>
        public virtual string SourceName
        {
            get { return _source; }
            set
            {
                if (_source == value)
                    return;

                _source = value;
                OnPropertyChanged("SourceName");
                OnPropertyChanged("Name");
            }
        }
        private string _source;

        /// <summary>
        /// Gets or sets the display name of this field.  Null means SourceName will be used instead.
        /// </summary>
        public virtual string DisplayName
        {
            get 
            {
                if (String.IsNullOrEmpty(_display))
                    return SourceName;
                return _display; 
            }
            set
            {
                if (_display == value)
                    return;

                _display = value;
                OnPropertyChanged("DisplayName");
                OnPropertyChanged("Name");
            }
        }
        private string _display;

        /// <summary>
        /// Gets or sets the display formate of this field.
        /// </summary>
        public virtual string DisplayFormat
        {
            get { return _format; }
            set
            {
                if (_format == value)
                    return;

                _format = value;
                OnPropertyChanged("DisplayFormat");
            }
        }
        private string _format;

        /// <summary>
        /// Gets or sets the type of data that can be found in this field.
        /// </summary>
        public virtual Type DataType
        {
            get { return _type; }
            set
            {
                if (_type == value)
                    return;

                if (value == null)
                    _type = typeof(object);
                else
                    _type = value;

                OnPropertyChanged("DataType");
            }
        }
        private Type _type;
        
        /// <summary>
        /// Gets the display name for this field, or the source name if display is null.
        /// </summary>
        public virtual string Name
        {
            get { return (String.IsNullOrEmpty(DisplayName) ? SourceName : DisplayName); }
        }

        /// <summary>
        /// Gets or sets an optional list of values that could be chosen for this field.
        /// </summary>
        public virtual IFieldList List
        {
            get { return _listData; }
            set
            {
                if (_listData == value)
                    return;

                _listData = value;
                OnPropertyChanged("ListData");
            }
        }
        private IFieldList _listData;

        /// <summary>
        /// Serializable constructor
        /// </summary>
        public Field()
            : this(null, null, null, typeof(object))
        {
        }

        public Field(ISubject parent, string sourceName)
            : this(parent, sourceName, null)
        {
        }

        public Field(string sourceName, Type type)
            : this(sourceName, null, type)
        {
        }

        public Field(string sourceName, string displayName, Type type)
            : this(null, sourceName, displayName, type)
        {
        }

        public Field(ISubject parent, string sourceName, string displayName)
            : this(parent, sourceName, displayName, typeof(object))
        {
        }

        public Field(ISubject parent, string sourceName, string displayName, Type type)
        {
            Subject = parent;
            SourceName = sourceName;
            DisplayName = displayName;
            DataType = type;
        }

        public Field(IField basedOn)
        {
            Subject = basedOn.Subject;
            SourceName = basedOn.SourceName;
            DisplayName = basedOn.DisplayName;
            DisplayFormat = basedOn.DisplayFormat;
            DataType = basedOn.DataType;
            
            // TODO: do we use this function?
            //if (List != null)
            //    List = (IFieldList)basedOn.List.Clone();
        }
        
        public override string ToString()
        {
            string prefix = "";
            if (Subject != null)
                prefix = String.Concat(Subject.DisplayName, ".");

            return String.Concat(prefix, Name);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
