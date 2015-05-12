using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace dbqf.Configuration
{
    /// <summary>
    /// Represents a view of the database to be used for user querying.
    /// </summary>
    public class ConfigurationImpl : IConfiguration
    {
        protected List<ISubject> _subjects;
        protected Dictionary<ISubject, Dictionary<ISubject, MatrixNode>> _matrix;

        public ConfigurationImpl()
        {
            _subjects = new List<ISubject>();
            _matrix = new Dictionary<ISubject, Dictionary<ISubject, MatrixNode>>();
        }

        public ISubject this[string displayName]
        {
            get
            {
                return _subjects.Find((s) => { return s.DisplayName.Equals(displayName); });
            }
        }

        public MatrixNode this[ISubject from, ISubject to]
        {
            get { return _matrix[from][to]; }
        }


        /// <summary>
        /// Fluently adds a new subject to the configuration.  If subject.ID is not set (&lt;= 0) it will be auto-set to the next count of Subjects.
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public IConfiguration Subject(ISubject subject)
        {
            Add(subject);
            return this;
        }

        public IConfiguration Matrix(ISubject from, ISubject to, string sql)
        {
            return Matrix(from, to, sql, null);
        }

        public IConfiguration Matrix(ISubject from, ISubject to, string sql, string tooltip)
        {
            var node = _matrix[from][to];
            node.Query = sql;
            node.ToolTip = tooltip;

            return this;
        }

        protected virtual void AddToMatrix(ISubject subject)
        {
            _matrix.Add(subject, new Dictionary<ISubject, MatrixNode>());
            foreach (var s in _subjects)
            {
                _matrix[subject].Add(s, new MatrixNode());

                if (!_matrix[s].ContainsKey(subject))
                    _matrix[s].Add(subject, new MatrixNode());
            }
        }

        protected virtual void RemoveFromMatrix(ISubject subject)
        {
            _matrix.Remove(subject);
            foreach (var s in _subjects)
            {
                if (_matrix[s].ContainsKey(subject))
                    _matrix[s].Remove(subject);
            }
        }


        #region IList Members

        public int IndexOf(ISubject item)
        {
            return _subjects.IndexOf(item);
        }

        public void Insert(int index, ISubject item)
        {
            _subjects.Insert(index, item);
            AddToMatrix(item);
        }

        public void RemoveAt(int index)
        {
            RemoveFromMatrix(_subjects[index]);
            _subjects.RemoveAt(index);
        }

        public ISubject this[int index]
        {
            get
            {
                return _subjects[index];
            }
            set
            {
                _subjects[index] = value;
            }
        }

        public void Add(ISubject item)
        {
            _subjects.Add(item);
            AddToMatrix(item);
            item.Configuration = this;
        }

        public void Clear()
        {
            _subjects.Clear();
            _matrix.Clear();
        }

        public bool Contains(ISubject item)
        {
            return _subjects.Contains(item);
        }

        public void CopyTo(ISubject[] array, int arrayIndex)
        {
            _subjects.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _subjects.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(ISubject item)
        {
            if (_subjects.Remove(item))
            {
                RemoveFromMatrix(item);
                return true;
            }

            return false;
        }

        public IEnumerator<ISubject> GetEnumerator()
        {
            return _subjects.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion


        //TODO: validate somewhere else


        ///// <summary>
        ///// Validate/setup configuration.
        ///// </summary>
        ///// <param name="setProperties">True to set properties in Field objects.</param>
        ///// <param name="queryer">Queryer to use if setting Field.ListData, null to ignore.</param>
        //protected void Validate(bool setProperties, Queryer queryer)
        //{
        //    // compile lookup of subjects by ID and ensure they are unique
        //    var lookup = new Dictionary<int, Subject>();
        //    foreach (Subject s in Subjects)
        //    {
        //        if (s.ID == 0)
        //            throw new ArgumentException(
        //                String.Concat("Subject ID's must start from 1.  Subject '", s.ToString(), "' has an ID value of zero."));

        //        if (lookup.ContainsKey(s.ID))
        //            throw new DuplicateKeyException(
        //                String.Concat("There are multiple subjects with the same ID.", Environment.NewLine, "Duplicate ID ", lookup[s.ID].ID, " between '", lookup[s.ID].ToString(), "' and '", s.ToString(), "'."));

        //        lookup.Add(s.ID, s);
        //    }

        //    // bind the Parent property on Field and ensure ComplexFields can be linked correctly.
        //    foreach (Subject s in Subjects)
        //    {
        //        bool hasID = false;
        //        bool defaultFound = false;
        //        foreach (Field f in s.Fields)
        //        {
        //            hasID |= (!(f is RelationManyField) && f.SourceName.Equals("ID"));
        //            defaultFound |= f.SourceName.Equals(s.DefaultFieldName);

        //            if (setProperties)
        //                f.Subject = s;

        //            if (f.Subject == null)
        //                throw new MissingSubjectException(String.Concat("Field ", f.ToString(), " Parent property cannot be null."));

        //            if (f.ListData != null && !String.IsNullOrEmpty(f.ListData.Source) && queryer != null)
        //            {
        //                try { f.ListData.Items = queryer.GetFieldList(f.ListData); }
        //                catch (Exception ex) { throw new ArgumentException(String.Concat("Could not initialise list data for ", f.ToString()), ex); }
        //            }

        //            if (f is RelationField)
        //            {
        //                int id = ((RelationField)f).LinkedSubjectID;
        //                if (id != 0)
        //                {
        //                    if (!lookup.ContainsKey(id))
        //                        throw new MissingSubjectException(
        //                            String.Concat("Complex field ", f.ToString(), " couldn't resolve subject with ID ", id));

        //                    if (setProperties)
        //                        ((RelationField)f).RelatedSubject = lookup[id];
        //                }
        //            }
        //        }

        //        if (!hasID)
        //            throw new MissingIDException(String.Concat("Subject ", s.ToString(), " must contain a field named 'ID'."));

        //        if (!String.IsNullOrEmpty(s.DefaultFieldName) && !defaultFound)
        //            throw new MissingFieldException(String.Concat("DefaultFieldName ", s.DefaultFieldName, " could not be found."));
        //    }


        //    // ensure the subject matrix is set up
        //    if (setProperties)
        //    {
        //        // ensure all subjects exist to/from in the subject matrix
        //        foreach (var pair in lookup)
        //        {
        //            if (!SubjectMatrix.ContainsKey(pair.Key))
        //                SubjectMatrix.Add(pair.Key, new SerializableDictionary<int, MatrixNode>());

        //            var mtx = SubjectMatrix[pair.Key];
        //            foreach (var pair2 in lookup)
        //            {
        //                if (!mtx.ContainsKey(pair2.Key))
        //                    mtx.Add(pair2.Key, MatrixNode.Empty);
        //                else if (mtx[pair2.Key] == null)
        //                    mtx[pair2.Key] = MatrixNode.Empty;
        //            }
        //        }
        //    }
        //}
    }
}
