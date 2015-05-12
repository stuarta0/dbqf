using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace DbQueryFramework_v1.Configuration
{
    /// <summary>
    /// A field is essentially a column in a database table but allows additional functionality like obtaining a list of possible values for the field.
    /// </summary>
    public class Field : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the source name of this field in the Subject's data.
        /// </summary>
        [XmlElement]
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
        [XmlElement]
        public virtual string DisplayName
        {
            get { return _display; }
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
        [XmlElement]
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
        /// Gets or sets whether this field should be output.
        /// </summary>
        [XmlElement]
        public virtual bool Output
        {
            get { return _output; }
            set
            {
                if (_output == value)
                    return;

                _output = value;
                OnPropertyChanged("Output");
            }
        }
        private bool _output;
        

        /// <summary>
        /// Gets or sets whether this field can be queried.
        /// </summary>
        [XmlElement]
        public virtual bool Queryable
        {
            get { return _queryable; }
            set
            {
                if (_queryable == value)
                    return;

                _queryable = value;
                OnPropertyChanged("Queryable");
            }
        }
        private bool _queryable;
        

        /// <summary>
        /// Gets the parent subject that contains this field.
        /// </summary>
        [XmlIgnore]
        public virtual Subject Parent
        {
            get { return _parent; }
            set
            {
                if (_parent == value)
                    return;

                _parent = value;
                OnPropertyChanged("Parent");
            }
        }
        private Subject _parent;


        /// <summary>
        /// Gets or sets the type of data that can be found in this field.
        /// </summary>
        [XmlIgnore]
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
                OnPropertyChanged("DataTypeName");
            }
        }
        private Type _type;
        

        /// <summary>
        /// Used for serialisation.
        /// </summary>
        [XmlElement("DataType")]
        public virtual string DataTypeName
        {
            get { return DataType.FullName; }
            set
            {
                if (DataType.FullName == value)
                    return;

                DataType = System.Type.GetType(value);
            }
        }
        

        /// <summary>
        /// Gets the display name for this field, or the source name if display is null.
        /// </summary>
        [XmlIgnore]
        public virtual string Name
        {
            get { return (String.IsNullOrEmpty(DisplayName) ? SourceName : DisplayName); }
        }


        /// <summary>
        /// Gets or sets an optional list of values that could be chosen for this field.
        /// </summary>
        [XmlElement]
        public virtual FieldList ListData
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
        private FieldList _listData;
        



        /// <summary>
        /// Serializable constructor
        /// </summary>
        public Field()
            : this(null, null)
        {
        }

        public Field(Subject parent, string sourceName)
            : this(parent, sourceName, null)
        {
        }

        public Field(Subject parent, string sourceName, string displayName)
        {
            Parent = parent;
            SourceName = sourceName;
            DisplayName = displayName;
            DataType = typeof(object);
            Output = true;
            Queryable = true;
        }

        public Field(Field basedOn)
        {
            Parent = basedOn.Parent;
            SourceName = basedOn.SourceName;
            DisplayName = basedOn.DisplayName;
            DisplayFormat = basedOn.DisplayFormat;
            DataType = basedOn.DataType;
            Output = basedOn.Output;
            Queryable = basedOn.Queryable;
            
            if (ListData != null)
                ListData = (FieldList)basedOn.ListData.Clone();
        }
        
        public override string ToString()
        {
            string prefix = "";
            if (Parent != null)
                prefix = String.Concat(Parent.DisplayName, ".");

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
