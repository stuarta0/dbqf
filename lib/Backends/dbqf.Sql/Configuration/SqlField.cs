
using dbqf.Configuration;
using System;

namespace dbqf.Sql.Configuration
{
    /// <summary>
    /// A field is essentially a column in a database table but allows additional functionality like obtaining a list of possible values for the field.
    /// </summary>
    public class SqlField : Field
    {
        /// <summary>
        /// Serializable constructor
        /// </summary>
        public SqlField() : base() { }

        public SqlField(ISqlSubject parent, string sourceName)
            : base(parent, sourceName, null) { }

        public SqlField(string sourceName, Type type)
            : base(sourceName, null, type) { }

        public SqlField(string sourceName, string displayName, Type type)
            : base(null, sourceName, displayName, type) { }

        public SqlField(ISqlSubject parent, string sourceName, string displayName)
            : base(parent, sourceName, displayName, typeof(object)) { }

        public SqlField(ISqlSubject parent, string sourceName, string displayName, Type type)
            : base(parent, sourceName, displayName, type) { }

        public SqlField(IField basedOn)
            : base(basedOn)
        {
            TypeCheck(basedOn.Subject);
        }

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

                TypeCheck(value);
                _parent = value;
                OnPropertyChanged("Subject");
            }
        }
        private ISubject _parent;

        protected virtual void TypeCheck(ISubject subject)
        {
            if (subject != null && !(subject is ISqlSubject))
                throw new ArgumentException("Subject must be of type ISqlSubject.");
        }

    }
}
