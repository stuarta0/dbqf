using dbqf.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace dbqf.Sql.Configuration
{
    /// <summary>
    /// Represents a view of the database to be used for user querying.
    /// </summary>
    public class MatrixConfiguration : IMatrixConfiguration
    {
        protected List<ISubject> _subjects;
        protected Dictionary<ISubject, Dictionary<ISubject, MatrixNode>> _matrix;

        public MatrixConfiguration()
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
            return MatrixSubject(subject);
        }

        public IMatrixConfiguration MatrixSubject(ISubject subject)
        {
            Add(subject);
            return this;
        }

        public IMatrixConfiguration Matrix(ISubject from, ISubject to, string sql)
        {
            return Matrix(from, to, sql, null);
        }

        public IMatrixConfiguration Matrix(ISubject from, ISubject to, string sql, string tooltip)
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
    }
}
