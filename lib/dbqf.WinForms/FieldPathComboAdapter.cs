using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace dbqf.WinForms
{
    /// <summary>
    /// Conditions:
    /// 1) No consumer should directly modify ComboSource, it should always be managed by the Adapter (should be ObservableReadOnlyCollection, .NET 3+)
    /// 2) FieldPath.Count == ComboSource.Count is always true
    /// 3) ComboSource[i].Contains(FieldPath[i]) is always true
    /// </summary>
    public class FieldPathComboAdapter
    {
        /// <summary>
        /// Gets the BindingList for use with the combos.  The outer list represents each combo, the inner list represents the combo data source.
        /// </summary>
        public BindingList<BindingList<IField>> ComboSource { get; private set; }

        /// <summary>
        /// Gets or sets the FieldPath that defines which combos will be displayed.
        /// </summary>
        public IFieldPath SelectedPath
        {
            get { return _path; }
            set
            {
                // check old path to new to see what new ComboSources we need to generate
                ComboSource.RaiseListChangedEvents = false;
                int indexOfChange = 0;
                if (value == null)
                {
                    ComboSource.Clear();
                    _path = value;
                }
                else if (_path != null)
                {
                    for (; indexOfChange < value.Count && indexOfChange < _path.Count && value[indexOfChange] == _path[indexOfChange]; indexOfChange++) ;
                    for (int i = ComboSource.Count - 1; i >= indexOfChange; i--)
                        ComboSource.RemoveAt(i);
                }
                _path = value;

                // if path is incomplete, complete it
                if (_path.Last is IRelationField)
                    _path.Add(FieldPath.FromDefault(_path.Last)[1, null]);

                // now fix up the combo sources
                for (int i = indexOfChange; i < _path.Count; i++)
                {
                    IList<IFieldPath> paths;
                    if (i > 0 && _path[i - 1] is IRelationField)
                        paths = _pathFactory.GetFields((IRelationField)_path[i - 1]);
                    else
                        paths = _pathFactory.GetFields(_path[i].Subject);

                    var fields = paths.Convert<IFieldPath, IField>(p => p[0]);
                    if (!fields.Contains(_path[i]))
                        ; // what do we do if the factory returns a list of fields that doesn't contain this part of the path?

                    ComboSource.Add(new BindingList<IField>(fields));
                }
                ComboSource.RaiseListChangedEvents = true;
                ComboSource.ResetBindings();
            }
        }
        private IFieldPath _path;

        private IFieldPathFactory _pathFactory;
        public FieldPathComboAdapter(IFieldPathFactory pathFactory)
        {
            _pathFactory = pathFactory;
            ComboSource = new BindingList<BindingList<IField>>();
        }
    }
}
