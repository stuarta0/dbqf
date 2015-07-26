using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace dbqf.Configuration
{
    /// <summary>
    /// Represents a subject in the database; usually maps directly to a database table.
    /// </summary>
    public class Subject : INotifyPropertyChanged, ISubject
    {
        /// <summary>
        /// Serializable constructor
        /// </summary>
        public Subject()
            : this(null)
        {
        }

        public Subject(string name)
        {
            DisplayName = name;
            _fields = new List<IField>();
        }

        public IConfiguration Configuration
        {
            get { return _configuration; }
            set
            {
                if (_configuration == value)
                    return;

                _configuration = value;
                OnPropertyChanged("Configuration");
            }
        }
        private IConfiguration _configuration;
        
        /// <summary>
        /// Gets or sets the source from where this subject data is retrieved.  
        /// In the case of an Sql configuration, Source would contain the SQL statement to retrieve results for this subject.
        /// </summary>
        public string Source
        {
            get { return _source; }
            set
            {
                if (_source == value)
                    return;

                _source = value;
                OnPropertyChanged("Source");
            }
        }
        private string _source;
        

        /// <summary>
        /// Gets or sets the display name of this subject.
        /// </summary>
        public string DisplayName
        {
            get { return _display; }
            set
            {
                if (_display == value)
                    return;

                _display = value;
                OnPropertyChanged("DisplayName");
            }
        }
        private string _display;

        /// <summary>
        /// Gets or sets the default field that should be the default field to use when querying this subject.  This field must exist in the Fields list.
        /// </summary>
        public IField DefaultField
        {
            get { return _defaultField; }
            set
            {
                if (_defaultField == value)
                    return;

                _defaultField = value;
                OnPropertyChanged("DefaultField");
            }
        }
        private IField _defaultField;

        /// <summary>
        /// Gets or sets the field that represents the key of this subject.  This field must exist in the Fields list.
        /// </summary>
        public IField IdField
        {
            get { return _idField; }
            set
            {
                if (_idField == value)
                    return;

                _idField = value;
                OnPropertyChanged("IdField");
            }
        }
        private IField _idField;

        /// <summary>
        /// Used for serialisation.
        /// </summary>
        public string DefaultFieldName
        {
            get { return _defaultFieldName; }
            set
            {
                if (_defaultFieldName == value)
                    return;

                _defaultFieldName = value;
                OnPropertyChanged("DefaultFieldName");
            }
        }
        private string _defaultFieldName;
        

        /// <summary>
        /// Gets the list of fields for this subject for serialisation.  The source name property for each field must represent a column output from this
        /// subject's Source property.  When the framework is processing this Subject and its fields, it will correlate the fields listed
        /// here with the output provided by the Source property.
        /// </summary>
        protected IList<IField> _fields;


        public IField this[string sourceName]
        {
            get
            {
                return _fields.Find((f) =>
                {
                    return f.SourceName.Equals(sourceName);
                });
            }
        }

        public override string ToString()
        {
            return DisplayName;
        }

        #region Fluent

        public Subject Sql(string sql)
        {
            Source = sql;
            return this;
        }

        public Subject Field(IField field)
        {
            Add(field);
            return this;
        }

        public Subject FieldId(IField field)
        {
            IdField = field;
            return Field(field);
        }

        public Subject FieldDefault(IField field)
        {
            DefaultField = field;
            DefaultFieldName = field.SourceName;
            return Field(field);
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IList Members

        public int IndexOf(IField item)
        {
            return _fields.IndexOf(item);
        }

        public void Insert(int index, IField item)
        {
            _fields.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _fields.RemoveAt(index);
        }

        public IField this[int index]
        {
            get
            {
                return _fields[index];
            }
            set
            {
                _fields[index] = value;
            }
        }

        public void Add(IField item)
        {
            _fields.Add(item);
            item.Subject = this;
        }

        public void Clear()
        {
            _fields.Clear();
        }

        public bool Contains(IField item)
        {
            return _fields.Contains(item);
        }

        public void CopyTo(IField[] array, int arrayIndex)
        {
            _fields.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _fields.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(IField item)
        {
            return _fields.Remove(item);
        }

        public IEnumerator<IField> GetEnumerator()
        {
            return _fields.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
