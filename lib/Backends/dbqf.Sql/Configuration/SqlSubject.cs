using dbqf.Configuration;
using System;

namespace dbqf.Sql.Configuration
{
    /// <summary>
    /// Represents a subject in the database; usually maps directly to a database table.
    /// </summary>
    public class SqlSubject : Subject, ISqlSubject
    {
        /// <summary>
        /// Serializable constructor
        /// </summary>
        public SqlSubject()
            : base() { }

        public SqlSubject(string name)
            : base(name) { }

        /// <summary>
        /// Gets or sets the source from where this subject data is retrieved.
        /// </summary>
        public string Sql
        {
            get { return _sql; }
            set
            {
                if (_sql == value)
                    return;

                _sql = value;
                OnPropertyChanged("Sql");
            }
        }
        private string _sql;
        
        public SqlSubject SqlQuery(string sql)
        {
            Sql = sql;
            return this;
        }

        #region Redefine Fluent interface

        public new SqlSubject Field(IField field)
        {
            return (SqlSubject)base.Field(field);
        }

        public new SqlSubject FieldId(IField field)
        {
            return (SqlSubject)base.FieldId(field);
        }

        public new SqlSubject FieldDefault(IField field)
        {
            return (SqlSubject)base.FieldDefault(field);
        }

        #endregion
    }
}
