using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using DbQueryFramework_v1.Actions;

namespace DbQueryFramework_v1.Configuration
{
    /// <summary>
    /// Represents a subject in the database; usually maps directly to a database table.
    /// </summary>
    public class Subject : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the unique identification of this subject.
        /// </summary>
        [XmlAttribute]
        public int ID
        {
            get { return _id; }
            set
            {
                if (_id == value)
                    return;

                _id = value;
                OnPropertyChanged("ID");
            }
        }
        private int _id;
        
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="DbQueryFramework.Configuration.Subject"/> is queryable.
		/// </summary>
		/// <value>
		/// <c>true</c> if queryable; otherwise, <c>false</c>.
		/// </value>
		[XmlElement]
		public bool Queryable 
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
        /// Gets or sets the source from where this subject data is retrieved.  This can be a database object name or a full SQL query.
        /// If the source is a table or view, then the framework will use a SELECT statement.  If the source is a stored procedure, then 
        /// the framework will use an EXEC statement.  Regardless of where the data comes from, the source must output a field named 'ID'.
        /// </summary>
        [XmlElement]
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
        [XmlElement]
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
        /// Gets or sets the bound at which this subject will be cached when querying.  A value less than zero means never cache; zero means always cache and greater than zero will be the number of times this subject will be queried before caching is applied.
        /// For example; if CacheBound = 2 and a query uses this subject once or twice then it won't be cached.  If this subject was used three times then the subject data would be cached and used by all three query parameters.
        /// </summary>
        /// <remarks>Recommended value is &lt;0 for most subjects (don't cache).  However, if the source takes some time to execute, consider setting this value to >=0 and check query performance.</remarks>
        [XmlElement]
        public int CacheBound
        {
            get { return _cache; }
            set
            {
                if (_cache == value)
                    return;

                _cache = value;
                OnPropertyChanged("CacheBound");
            }
        }
        private int _cache;


        /// <summary>
        /// Gets or sets the default field name (Field.SourceName) that should be the default field to use when querying this subject.  This field must be a Queryable field.
        /// </summary>
        [XmlElement]
        public string DefaultFieldName
        {
            get { return _defaultField; }
            set
            {
                if (_defaultField == value)
                    return;

                _defaultField = value;
                OnPropertyChanged("CacheBound");
            }
        }
        private string _defaultField;
        

        /// <summary>
        /// Gets the list of fields for this subject.  The source name property for each field must represent a column output from this
        /// subject's Source property.  When the framework is processing this Subject and its fields, it will correlate the fields listed
        /// here with the output provided by the Source property.
        /// </summary>
        [XmlArray]
        [XmlArrayItem(typeof(Field))]
        [XmlArrayItem(typeof(ComplexField))]
        [XmlArrayItem(typeof(ComplexManyField))]
        public List<Field> Fields { get; set; }


        /// <summary>
        /// Specifies a list of possible actions to perform when an item is selected in the user interface.
        /// </summary>
        [XmlArray]
        [XmlArrayItem(typeof(UriAction))]
        [XmlArrayItem(typeof(ProcessAction))]
        public List<SubjectAction> Actions { get; set; }



        /// <summary>
        /// Serializable constructor
        /// </summary>
        public Subject()
            : this(null)
        {
        }

        public Subject(string source)
            : this(0, source, null)
        {
        }

        public Subject(int id, string source, string name)
        {
            ID = id;
            Source = source;
            DisplayName = name;
            Fields = new List<Field>();
            CacheBound = -1; // default to never cache
            Queryable = true; // default to queryable
        }

        public Field GetField(string sourceName)
        {
            return Fields.Find((f) => 
            {
                bool found = false;
                if (f is ComplexManyField)
                    found = ((ComplexManyField)f).DisplayName.Equals(sourceName);
                else if (f is ComplexField)
                    found = ((ComplexField)f).OutputSourceName.Equals(sourceName);
                
                return found || f.SourceName.Equals(sourceName); 
            });
        }

        public Field GetFieldByDisplayName(string displayName)
        {
            return Fields.Find((f) => 
            {
                bool found = false;
                if (f is ComplexManyField)
                    found = ((ComplexManyField)f).DisplayName.Equals(displayName);
                
                return found || (f.DisplayName == null ? f.SourceName : f.DisplayName).Equals(displayName);
            });
        }

        public override string ToString()
        {
            return DisplayName;
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
