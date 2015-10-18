using dbqf.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace dbqf.Sql.Configuration
{
    /// <summary>
    /// Represents a view of the database to be used for user querying.  Must be used with ISqlSubject types.
    /// </summary>
    public class MatrixConfiguration : IMatrixConfiguration
    {
        protected List<ISqlSubject> _subjects;
        protected Dictionary<ISqlSubject, Dictionary<ISqlSubject, MatrixNode>> _matrix;

        public MatrixConfiguration()
        {
            _subjects = new List<ISqlSubject>();
            _matrix = new Dictionary<ISqlSubject, Dictionary<ISqlSubject, MatrixNode>>();
        }

        public ISubject this[string displayName]
        {
            get
            {
                return _subjects.Find((s) => { return s.DisplayName.Equals(displayName); });
            }
        }

        public MatrixNode this[ISqlSubject from, ISqlSubject to]
        {
            get { return _matrix[from][to]; }
        }


        private void TypeCheck(ISubject item)
        {
            if (!(item is ISqlSubject))
                throw new ArgumentException("Subjects must be of type ISqlSubject.");
        }

        /// <summary>
        /// Fluently adds a new subject to the configuration.
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public MatrixConfiguration Subject(ISqlSubject subject)
        {
            Add(subject);
            return this;
        }

        public MatrixConfiguration Matrix(ISqlSubject from, ISqlSubject to, string sql)
        {
            return Matrix(from, to, sql, null);
        }

        public MatrixConfiguration Matrix(ISqlSubject from, ISqlSubject to, string sql, string tooltip)
        {
            var node = _matrix[from][to];
            node.Query = sql;
            node.ToolTip = tooltip;

            return this;
        }

        protected virtual void AddToMatrix(ISqlSubject subject)
        {
            _matrix.Add(subject, new Dictionary<ISqlSubject, MatrixNode>());
            foreach (var s in _subjects)
            {
                _matrix[subject].Add(s, new MatrixNode());

                if (!_matrix[s].ContainsKey(subject))
                    _matrix[s].Add(subject, new MatrixNode());
            }
        }

        protected virtual void RemoveFromMatrix(ISqlSubject subject)
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
            TypeCheck(item);
            return _subjects.IndexOf((ISqlSubject)item);
        }

        public void Insert(int index, ISubject item)
        {
            TypeCheck(item);
            _subjects.Insert(index, (ISqlSubject)item);
            AddToMatrix((ISqlSubject)item);
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
                TypeCheck(value);
                _subjects[index] = (ISqlSubject)value;
            }
        }

        public void Add(ISubject item)
        {
            TypeCheck(item);
            _subjects.Add((ISqlSubject)item);
            AddToMatrix((ISqlSubject)item);
            item.Configuration = this;
        }

        public void Clear()
        {
            _subjects.Clear();
            _matrix.Clear();
        }

        public bool Contains(ISubject item)
        {
            if (item is ISqlSubject)
                return _subjects.Contains((ISqlSubject)item);
            return false;
        }

        public void CopyTo(ISubject[] array, int arrayIndex)
        {
            for (int i = 0; arrayIndex + i < array.Length && i < _subjects.Count; i++)
                array[arrayIndex + i] = _subjects[i];
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
            if (item is ISqlSubject && _subjects.Remove((ISqlSubject)item))
            {
                RemoveFromMatrix((ISqlSubject)item);
                return true;
            }

            return false;
        }

        public IEnumerator<ISubject> GetEnumerator()
        {
            foreach (var s in _subjects)
                yield return s;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
