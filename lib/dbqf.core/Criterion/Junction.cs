using dbqf.Configuration;
using dbqf.Processing;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Criterion
{
    public abstract class Junction : IParameter, IList<IParameter>
    {
        private IList<IParameter> _parameters;

        protected abstract string Op { get; }

        public Junction()
        {
            _parameters = new List<IParameter>();
        }

        /// <summary>
        /// Fluently add a parameter to this junction.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Junction Parameter(IParameter parameter)
        {
           Add(parameter);
            return this;
        }

        public SqlString ToSqlString()
        {
            var sql = new SqlString();
            for (int i = 0; i < _parameters.Count; i++)
            {
                // add the parameters if there's something to add
                var p = _parameters[i].ToSqlString().Flatten();
                if (p.Parts.Count > 0)
                    sql.Add(p);
            }

            // insert the operator between all the parts
            for (int i = 0; i < sql.Parts.Count - 1; i += 2)
                sql.Parts.Insert(i + 1, String.Concat(" ", Op, " "));

            // if we managed to produce something, wrap it in parens
            if (sql.Parts.Count > 0)
            {
                sql.Parts.Insert(0, "(");
                sql.Parts.Add(")");
            }

            return sql;
        }

        public int IndexOf(IParameter item)
        {
            return _parameters.IndexOf(item);
        }

        public void Insert(int index, IParameter item)
        {
            _parameters.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _parameters.RemoveAt(index);
        }

        public IParameter this[int index]
        {
            get
            {
                return _parameters[index];
            }
            set
            {
                _parameters[index] = value;
            }
        }

        public void Add(IParameter item)
        {
            _parameters.Add(item);
        }

        public void Clear()
        {
            _parameters.Clear();
        }

        public bool Contains(IParameter item)
        {
            return _parameters.Contains(item);
        }

        public void CopyTo(IParameter[] array, int arrayIndex)
        {
            _parameters.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _parameters.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(IParameter item)
        {
            return _parameters.Remove(item);
        }

        public IEnumerator<IParameter> GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
