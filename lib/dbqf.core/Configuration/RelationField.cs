using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace dbqf.Configuration
{
    /// <summary>
    /// A field that relates to another Subject.  This can be used to traverse relationships in the configuration.
    /// </summary>
    public class RelationField : Field, IRelationField
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

                _relatedSubject = value;
                OnPropertyChanged("RelatedSubject");
            }
        }
        private ISubject _relatedSubject;


        /// <summary>
        /// Serialization constructor
        /// </summary>
        public RelationField()
            : this(null, null)
        {
        }

        public RelationField(ISubject parent, string sourceName)
            : this(parent, sourceName, null)
        {
        }

        public RelationField(ISubject parent, string sourceName, string displayName)
            : this(parent, sourceName, displayName, null)
        {
        }

        public RelationField(ISubject parent, string sourceName, string displayName, ISubject linkedSubject)
            : this(parent, sourceName, displayName, typeof(object), linkedSubject)
        {
        }

        /// <summary>
        /// Set up a complex field and automatically set this field's DataType to the type of the linked subjects ID field.
        /// </summary>
        public RelationField(string sourceName, string displayName, ISubject linkedSubject)
            : this(null, sourceName, displayName, null, linkedSubject)
        {
            DataType = linkedSubject.IdField.DataType;
        }

        public RelationField(string sourceName, string displayName, Type type, ISubject linkedSubject)
            : this(null, sourceName, displayName, type, linkedSubject)
        {
        }
            
        public RelationField(ISubject parent, string sourceName, string displayName, Type type, ISubject linkedSubject)
            : base(parent, sourceName, displayName, type)
        {
            RelatedSubject = linkedSubject;
        }

        public RelationField(IField basedOn)
            : base(basedOn)
        {
            if (basedOn is IRelationField)
                RelatedSubject = ((IRelationField)basedOn).RelatedSubject;
            else
                RelatedSubject = null;
        }
    }
}
