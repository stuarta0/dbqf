using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DbQueryFramework_v1.Configuration
{
    /// <summary>
    /// A field that relates to another Subject.  This can be used to traverse relationships in the configuration.
    /// </summary>
    [Serializable]
    public class ComplexField : Field
    {
        /// <summary>
        /// The subject that this field relates to.  This is used to further 'drill-down' a field that represents a complex type 
        /// (i.e. isn't a simple string or number)
        /// </summary>
        [XmlIgnore]
        public virtual Subject LinkedSubject
        {
            get { return _linkedSubject; }
            set
            {
                if (_linkedSubject == value)
                    return;

                _linkedSubject = value;
                OnPropertyChanged("LinkedSubject");

                if (_linkedSubject != null)
                    LinkedSubjectID = _linkedSubject.ID;
            }
        }
        private Subject _linkedSubject;


        /// <summary>
        /// Used for serialization to resolve the subject later.
        /// </summary>
        [XmlAttribute]
        public virtual int LinkedSubjectID 
        { 
            get { return _linkedID; }
            set
            {
                if (_linkedID == value)
                    return;

                _linkedID = value;
                OnPropertyChanged("LinkedSubjectID");
            }
        }
        private int _linkedID;

        /// <summary>
        /// Gets or sets the source name of this field in the Subject's data that will be used if this field is output.
        /// </summary>
        [XmlElement]
        public virtual string OutputSourceName
        {
            get { return _outputSource; }
            set
            {
                if (_outputSource == value)
                    return;

                _outputSource = value;
                OnPropertyChanged("OutputSourceName");
            }
        }
        private string _outputSource;


        /// <summary>
        /// Gets or sets the type of data that can be found in the output field.
        /// </summary>
        [XmlIgnore]
        public virtual Type OutputDataType
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

                OnPropertyChanged("OutputDataType");
                OnPropertyChanged("OutputDataTypeName");
            }
        }
        private Type _type;


        /// <summary>
        /// Used for serialisation.
        /// </summary>
        [XmlElement("OutputDataType")]
        public virtual string OutputDataTypeName
        {
            get 
            {
                if (OutputDataType == null)
                    return string.Empty;

                return OutputDataType.FullName; 
            }
            set
            {
                if (OutputDataType != null && OutputDataType.FullName == value)
                    return;

                OutputDataType = System.Type.GetType(value);
            }
        }


        /// <summary>
        /// Serialization constructor
        /// </summary>
        public ComplexField()
            : this(null, null)
        {
        }

        public ComplexField(Subject parent, string sourceName)
            : this(parent, sourceName, null)
        {
        }

        public ComplexField(Subject parent, string sourceName, string displayName)
            : this(parent, sourceName, displayName, null)
        {
        }

        public ComplexField(Subject parent, string sourceName, string displayName, Subject linkedSubject)
            : base(parent, sourceName, displayName)
        {
            LinkedSubject = linkedSubject;
            Output = false;
            OutputSourceName = "";
        }

        public ComplexField(Field basedOn)
            : base(basedOn)
        {
            if (basedOn is ComplexField)
            {
                LinkedSubject = ((ComplexField)basedOn).LinkedSubject;
                Output = ((ComplexField)basedOn).Output;
                OutputSourceName = ((ComplexField)basedOn).OutputSourceName;
                OutputDataType = ((ComplexField)basedOn).OutputDataType;
            }
            else
            {
                LinkedSubject = null;
                Output = false;
                OutputSourceName = "";
            }
        }

        /// <summary>
        /// Starting from this field, navigate subsequent default fields for subjects until a normal (non-complex) field is found.
        /// NOTE: This could cause an infinite loop depending on the configuration.
        /// </summary>
        /// <returns>A list of fields including this field from here to a standard field.</returns>
        public List<Field> GetPathToDefaultField()
        {
            var list = new List<Field>();
            Field cur = this;

            while (cur != null)
            {
                list.Add(cur);
                if (cur is ComplexField)
                {
                    var subject = ((ComplexField)cur).LinkedSubject;
                    cur = subject.GetField(subject.DefaultFieldName);
                }
                else
                    cur = null;
            }

            return list;
        }
    }
}
