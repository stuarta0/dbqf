using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Collections.ObjectModel;

namespace dbqf.Display
{
    /// <summary>
    /// Contains a mechanism to maintain relationship between a SelectedPath and a list of available fields for each index in the path.
    /// The intent is to allow a user to drill through the data via ComboBoxes (bound to Items), with consumers using the resulting SelectedPath.
    /// 
    /// Conditions:
    /// 1) Items is maintained internally and should not be modified by consumers (should be ObservableReadOnlyCollection, .NET 3+)
    /// 2) Each index in Items will corresponding to the index in SelectedPath. i.e. SelectedPath[i] == Items[i].SelectedField
    /// 3) Items.Count == SelectedPath.Count
    /// </summary>
    public class FieldPathComboAdapter : IFieldPathComboBox
    {
        /// <summary>
        /// Gets the BindingList for use with the combos.
        /// </summary>
        public BindingList<FieldPathComboItem> Items { get; private set; }

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
                    Items.Clear();
                else if (_path != null)
                {
                    for (; indexOfChange < value.Count && indexOfChange < _path.Count && value[indexOfChange] == _path[indexOfChange]; indexOfChange++) ;
                    for (int i = Items.Count - 1; i >= indexOfChange; i--)
                    {
                        Items[i].SelectedFieldChanged -= SelectedFieldChanged;
                        Items.RemoveAt(i);
                    }
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

                    // what do we do if the factory returns a list of fields that doesn't contain this part of the path?
                    var fields = paths.Convert<IFieldPath, IField>(p => p[0]);
                    if (!fields.Contains(_path[i]))
                        throw new ArgumentException(String.Format("FieldPathFactory returned a list of fields that does not contain the desired field {0}.", _path[i].DisplayName)); 

                    var d = new FieldPathComboItem(fields, _path[i]);
                    d.SelectedFieldChanged += SelectedFieldChanged;
                    Items.Add(d);
                }

                OnSelectedPathChanged();
            }
        }
        private IFieldPath _path;

        public event EventHandler SelectedPathChanged = delegate { };
        private void OnSelectedPathChanged()
        {
            SelectedPathChanged(this, EventArgs.Empty);
        }

        void SelectedFieldChanged(object sender, EventArgs e)
        {
            var d = ((FieldPathComboItem)sender);
            var path = new FieldPath();
            foreach (var f in Items)
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
            Items = new BindingList<FieldPathComboItem>();
        }
    }

    /// <summary>
    /// Represents a list of fields and the selected field at a certain depth through a FieldPath.
    /// Fields would be the data source of the ComboBox, SelectedField is the selected item of the ComboBox.
    /// FieldPathComboAdapter utilizes this class by having a collection of these, with the overall SelectedPath representing the result of this structure.
    /// </summary>
    public class FieldPathComboItem
    {
        public BindingList<IField> Fields { get; private set; }

        public IField SelectedField
        {
            get { return _selected; }
            set 
            { 
                _selected = value;
                OnSelectedFieldChanged();
            }
        }
        private IField _selected;

        public event EventHandler SelectedFieldChanged = delegate { };
        private void OnSelectedFieldChanged()
        {
            SelectedFieldChanged(this, EventArgs.Empty);
        }

        public FieldPathComboItem(IList<IField> fields, IField selected = null)
        {
            Fields = new BindingList<IField>(fields);
            SelectedField = selected == null ? Fields[0] : selected;
        }
    }
}
