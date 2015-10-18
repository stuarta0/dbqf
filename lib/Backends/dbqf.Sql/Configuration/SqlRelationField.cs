using dbqf.Configuration;
using System;

namespace dbqf.Sql.Configuration
{
    /// <summary>
    /// A field that relates to another Subject.  This can be used to traverse relationships in the configuration.
    /// </summary>
    public class SqlRelationField : SqlField, ISqlRelationField
    {
        /// <summary>
        /// The subject that this field relates to.  This is used to further 'drill-down' a field that represents a complex type 
        /// (i.e. isn't a simple string or number)
        /// </summary>
        public virtual ISubject RelatedSubject
        {
            get { return _relatedSubject; }
            set
            {
                if (_relatedSubject == value)
                    return;

                base.TypeCheck(value);
                _relatedSubject = value;
                OnPropertyChanged("RelatedSubject");
            }
        }
        private ISubject _relatedSubject;


        /// <summary>
        /// Serialization constructor
        /// </summary>
        public SqlRelationField()
            : this(null, null)
        {
        }

        public SqlRelationField(ISqlSubject parent, string sourceName)
            : this(parent, sourceName, null)
        {
        }

        public SqlRelationField(ISqlSubject parent, string sourceName, string displayName)
            : this(parent, sourceName, displayName, null)
        {
        }

        public SqlRelationField(ISqlSubject parent, string sourceName, string displayName, ISqlSubject linkedSubject)
            : this(parent, sourceName, displayName, typeof(object), linkedSubject)
        {
        }

        /// <summary>
        /// Set up a complex field and automatically set this field's DataType to the type of the linked subjects ID field.
        /// </summary>
        public SqlRelationField(string sourceName, string displayName, ISqlSubject linkedSubject)
            : this(null, sourceName, displayName, null, linkedSubject)
        {
            DataType = linkedSubject.IdField.DataType;
        }

        public SqlRelationField(string sourceName, string displayName, Type type, ISqlSubject linkedSubject)
            : this(null, sourceName, displayName, type, linkedSubject)
        {
        }
            
        public SqlRelationField(ISqlSubject parent, string sourceName, string displayName, Type type, ISqlSubject linkedSubject)
            : base(parent, sourceName, displayName, type)
        {
            RelatedSubject = linkedSubject;
        }

        public SqlRelationField(IField basedOn)
            : base(basedOn)
        {
            if (basedOn is IRelationField && ((IRelationField)basedOn).Subject is ISqlSubject)
                RelatedSubject = ((IRelationField)basedOn).RelatedSubject;
            else
                RelatedSubject = null;
        }
    }
}
