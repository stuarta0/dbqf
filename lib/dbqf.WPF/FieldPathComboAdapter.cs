using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Collections.ObjectModel;
using PropertyChanged;

namespace dbqf.WPF
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
        public ObservableCollection<FieldPathDepth> Depth { get; private set; }

        /// <summary>
        /// Gets or sets the FieldPath that defines which combos will be displayed.
        /// </summary>
        public IFieldPath SelectedPath
        {
            get { return _path; }
            set
            {
                int indexOfChange = 0;
                if (value == null)
                    Depth.Clear();
                else if (_path != null)
                {
                    for (; indexOfChange < value.Count && indexOfChange < _path.Count && value[indexOfChange] == _path[indexOfChange]; indexOfChange++) ;
                    for (int i = Depth.Count - 1; i >= indexOfChange; i--)
                    {
                        Depth[i].SelectedFieldChanged -= SelectedFieldChanged;
                        Depth.RemoveAt(i);
                    }
                }
                _path = value;

                // if path is incomplete, complete it
                if (_path.Last is IRelationField)
                    _path.Add(FieldPath.FromDefault(_path.Last)[1, null]);

                // now fix up the combo sources
                for (int i = indexOfChange; i < _path.Count; i++)
                {
                    var d = new FieldPathDepth(_pathFactory.GetFields(_path[i].Subject).Convert<IFieldPath, IField>(p => p[0]), _path[i]);
                    d.SelectedFieldChanged += SelectedFieldChanged;
                    Depth.Add(d);
                }
            }
        }
        private IFieldPath _path;

        void SelectedFieldChanged(object sender, EventArgs e)
        {
            var d = ((FieldPathDepth)sender);
            var path = new FieldPath();
            foreach (var f in Depth)
            {
                path.Add(f.SelectedField);
                if (f == d) // end of the road (either truncated or the last field changed)
                {
                    SelectedPath = path;
                    return;
                }
            }
        }

        private IFieldPathFactory _pathFactory;
        public FieldPathComboAdapter(IFieldPathFactory pathFactory)
        {
            _pathFactory = pathFactory;
            Depth = new ObservableCollection<FieldPathDepth>();
        }
    }

    [ImplementPropertyChanged]
    public class FieldPathDepth
    {
        public ObservableCollection<IField> Fields { get; private set; }
        public IField SelectedField { get; set; }
        public event EventHandler SelectedFieldChanged = delegate { };
        public void OnSelectedFieldChanged()
        {
            SelectedFieldChanged(this, EventArgs.Empty);
        }

        public FieldPathDepth(IEnumerable<IField> fields, IField selected = null)
        {
            Fields = new ObservableCollection<IField>(fields);
            SelectedField = selected == null ? Fields[0] : selected;
        }
    }
}
