using dbqf.Configuration;
using dbqf.Processing;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Criterion
{
    public abstract class Junction : IJunction, IList<IParameter>
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
        public virtual Junction Parameter(IParameter parameter)
        {
           Add(parameter);
            return this;
        }

        public virtual int IndexOf(IParameter item)
        {
            return _parameters.IndexOf(item);
        }

        public virtual void Insert(int index, IParameter item)
        {
            _parameters.Insert(index, item);
        }

        public virtual void RemoveAt(int index)
        {
            _parameters.RemoveAt(index);
        }

        public virtual IParameter this[int index]
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

        public virtual void Add(IParameter item)
        {
            _parameters.Add(item);
        }

        public virtual void Clear()
        {
            _parameters.Clear();
        }

        public virtual bool Contains(IParameter item)
        {
            return _parameters.Contains(item);
        }

        public virtual void CopyTo(IParameter[] array, int arrayIndex)
        {
            _parameters.CopyTo(array, arrayIndex);
        }

        public virtual int Count
        {
            get { return _parameters.Count; }
        }

        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        public virtual bool Remove(IParameter item)
        {
            return _parameters.Remove(item);
        }

        public virtual IEnumerator<IParameter> GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
