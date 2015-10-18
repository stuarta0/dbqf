using dbqf.Configuration;
using System;

namespace dbqf.Sql.Configuration
{
    /// <summary>
    /// Represents a subject in the database; usually maps directly to a database table.
    /// </summary>
    public class SqlSubject : Subject
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
        /// In the case of an Sql configuration, Source would contain the SQL statement to retrieve results for this subject.
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


        #region Type-checking base class overrides

        private void TypeCheck(IField field)
        {
            if (!(field is ISqlField))
                throw new ArgumentException("Fields must be of type ISqlField.");
        }

        public override void Insert(int index, IField item)
        {
            TypeCheck(item);
            base.Insert(index, item);
        }

        public override void Add(IField item)
        {
            TypeCheck(item);
            base.Add(item);
        }

        public override Subject Field(IField field)
        {
            TypeCheck(field);
            return base.Field(field);
        }

        public override Subject FieldDefault(IField field)
        {
            TypeCheck(field);
            return base.FieldDefault(field);
        }

        public override Subject FieldId(IField field)
        {
            TypeCheck(field);
            return base.FieldId(field);
        }

        public override IField this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                TypeCheck(value);
                base[index] = value;
            }
        }

        #endregion
    }
}
