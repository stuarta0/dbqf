using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DbQueryFramework_v1.Configuration
{
    /// <summary>
    /// ComplexManyField represents a field that relates to many items in the LinkedSubject object.
    /// For example: User -> Permissions is a list of permissions assigned to a User (where Permission is a Subject) as opposed to User -> Manager which is a one to one relationship (where Manager is also a User subject).
    /// </summary>
    [Serializable]
    public class ComplexManyField : ComplexField
    {
        /// <summary>
        /// SourceName is fixed to "ID" and cannot be changed for this field type.
        /// </summary>
        [XmlIgnore]
        public override string SourceName
        {
            get { return "ID"; }
            set { base.SourceName = value; }
        }

        /// <summary>
        /// DisplayName is the LinkedSubject's Name unless overriden.
        /// </summary>
        [XmlIgnore]
        public override string DisplayName
        {
            get 
            {
                if (String.IsNullOrEmpty(base.DisplayName) && LinkedSubject != null)
                    return LinkedSubject.DisplayName;

                return base.DisplayName; 
            }
            set { base.DisplayName = value; }
        }

        /// <summary>
        /// Output is fixed to false as this type of field would cause confusion in standard output.
        /// </summary>
        [XmlIgnore]
        public override bool Output
        {
            get { return false; }
            set { base.Output = value; }
        }

        /// <summary>
        /// DataType is fixed to the type of this.Parent.ID field.
        /// </summary>
        [XmlIgnore]
        public override Type DataType
        {
            get { return base.DataType; }
            set { base.DataType = value; }
        }

        /// <summary>
        /// DataTypeName is fixed to the type name of this.Parent.ID field.
        /// </summary>
        [XmlIgnore]
        public override string DataTypeName
        {
            get { return base.DataTypeName; }
            set { base.DataTypeName = value; }
        }

                

        /// <summary>
        /// Serialization constructor
        /// </summary>
        public ComplexManyField()
            : this((Subject)null)
        {
        }

        public ComplexManyField(Subject parent)
            : this(parent, null)
        {
        }

        public ComplexManyField(Subject parent, string displayName)
            : this(parent, displayName, null)
        {
        }

        public ComplexManyField(Subject parent, string displayName, Subject linkedSubject)
            : base(parent, "ID", displayName)
        {
            Output = false;
        }

        public ComplexManyField(Field basedOn)
            : base(basedOn)
        {
            if (basedOn is ComplexManyField)
                Output = ((ComplexManyField)basedOn).Output;
            else
                Output = false;
        }
    }
}
