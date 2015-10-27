using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Sql.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace dbqf.Sql
{
    /// <summary>
    /// A recursive dictionary to store unique join paths for a database query.
    /// </summary>
    [DebuggerDisplay("{Subject} : {Field}")]
    public class UniqueFieldPath : IEnumerable<UniqueFieldPath>
    {
        public UniqueFieldPath Predecessor { get; private set; }
        public string Alias { get; set; }
        protected Dictionary<IRelationField, UniqueFieldPath> _paths;

        public IRelationField Field 
        { 
            get 
            {
                if (_field is IRelationField)
                    return (IRelationField)_field;
                return null;
            } 
        }
        private IField _field;

        /// <summary>
        /// If the field is an IRelationField, it's business as usual.  If it's not, we're at the root and field is the ID field.
        /// </summary>
        public ISqlSubject Subject
        {
            get
            {
                if (_field is IRelationField)
                    return (ISqlSubject)((IRelationField)_field).RelatedSubject;
                return (ISqlSubject)_field.Subject;
            }
        }

        public UniqueFieldPath(ISqlSubject root)
        {
            _field = root.IdField;
            _paths = new Dictionary<IRelationField, UniqueFieldPath>();
        }

        public UniqueFieldPath(IRelationField field)
        {
            _field = field;
            _paths = new Dictionary<IRelationField, UniqueFieldPath>();
        }

        public virtual void Add(IFieldPath path)
        {
            // nothing to do if we're at the end of the path
            if (path.Count <= 1)
                return;

            var field = (IRelationField)path[0];
            if (field.Subject.Equals(Subject))
            {
                if (!_paths.ContainsKey(field))
                    _paths.Add(field, new UniqueFieldPath(field) {Predecessor = this });
                _paths[field].Add(path[1,null]);
            }
            else
                throw new ArgumentException(String.Format("Cannot add given FieldPath to UniqueFieldPath as parent subjects at this point differ ({0}, trying to add {1}).", Field.ToString(), path[0].ToString()));
        }

        public virtual UniqueFieldPath this[IFieldPath key]
        {
            get
            {
                // unwrap FieldPath recursively to find the last step, assuming FieldPath.Last is IField (i.e. we don't traverse it)
                if (key.Count == 1)
                    return this;
                return _paths[(IRelationField)key[0]][key[1,null]];
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is IField)
                return Field.Equals(obj);
            return base.Equals(obj);
        }

        public IEnumerator<UniqueFieldPath> GetEnumerator()
        {
            foreach (var v in _paths.Values)
                yield return v;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// Works with ISqlSubject, ISqlField, and ISqlRelationField.
    /// 
    /// Simply a container to provide a way of keeping disconnected subjects together, i.e.
    /// if we had 3 different paths to add: Products.Category.Name = X, Products.Name = Y, Invoice.Date = Z
    /// in this example the UniqueFieldPaths object would contain a list of 2 UniqueFieldPath objects, 1 for Products root, and 1 for Invoice root.
    /// </summary>
    public class UniqueFieldPaths : IEnumerable<UniqueFieldPath>
    {
        protected List<UniqueFieldPath> _paths;

        private UniqueFieldPaths()
        {
            _paths = new List<UniqueFieldPath>();
        }

        /// <summary>
        /// Create a RootFieldPath from a list of various paths.  Indexing this root path will result in finding the correct (unique) field path.
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static UniqueFieldPaths FromPaths(IList<IFieldPath> paths)
        {
            var root = new UniqueFieldPaths();
            foreach (var path in paths)
            {
                var subject = (ISqlSubject)path[0].Subject;
                var uniquePath = root._paths.Find(m => m.Subject.Equals(subject));
                if (uniquePath == null)
                    root._paths.Add(uniquePath = new UniqueFieldPath(subject));

                uniquePath.Add(path);
            }
            return root;
        }

        public virtual UniqueFieldPath this[IFieldPath key]
        {
            get
            {
                var ufp = _paths.Find(m => m.Subject.Equals(key[0].Subject));
                if (ufp == null)
                    throw new KeyNotFoundException(String.Format("FieldPath starting at {0} not found in unique field path", key[0].ToString()));

                return ufp[key];
            }
        }

        public IEnumerator<UniqueFieldPath> GetEnumerator()
        {
            return _paths.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
