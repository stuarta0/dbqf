using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace dbqf.WinForms
{
    public class FieldPathComboAdapter
    {
        // e.g. From a set of fields from Invoice:
        //   Combo1: Authoriser (IRelationField to Person)
        //   Combo2: Superior (IRelationField to Person)
        //   Combo3: Name

        // Anytime an IRelationField is chose, the FieldPathFactory will be asked for the next set of fields and added to the ComboSource list
        // e.g. from Invoice:
        //   Combo1: Number
        // currently ComboSource looks like: 
        //   [0] = BindingList[10] (invoice), selected field is Number

        // then the user selects Authoriser
        //   Combo1: Authoriser
        //   Combo2: Name *New
        // upon choosing Authoriser, FieldPathFactory is asked for the next set of fields, plus the RelatedSubject.DefaultField is recursively set
        // for the next level (it might be deeper than 1 level, requiring more combos)
        // currently ComboSource looks like:
        //   [0] = BindingList[10] (invoice), selected field is Authoriser
        //   [1] = BindingList[4] (person), selected field is Name

        // now the user chooses Superior
        //   Combo1: Authoriser
        //   Combo2: Superior
        //   Combo3: Name
        // again, FieldPathFactory is asked for fields after superior, plus DefaultField is set
        // currently ComboSource looks like:
        //   [0] = BindingList[10] (invoice), selected field is Authoriser
        //   [1] = BindingList[4] (person), selected field is Superior
        //   [2] = BindingList[4] (person), selected field is Name

        // now lets say the first combo is set back to Number:
        //   Combo1: Number
        // since it's not an IRelationField, all indicies after ComboSource[0] are cleared
        // currently ComboSource looks like:
        //   [0] = BindingList[10] (invoice), selected field is Number



        // So how does this propogate between the adapter and UI?
        /*
         * UI hooks ComboSource ListChanged upon which it loops and builds it's combos accordingly (each index is a new combo with it's source as ComboSource[i])
         *   The selected item in each combo is based on the Adapter.SelectedPath[i].
         * Upon changing any combo, the UI tells the adapter to update to a FieldPath consisting of a path built from the field in the combo that changed, plus
         *   all the fields up until this point (i.e. loop in reverse over the combos and Insert(0, value) into the FieldPath).  
         * When the Adapter updates from a FieldPath, it updates it's ComboSource, adding any additional depth required (i.e. if the last field in the path
         *   is an IRelationField, it needs to add more fields until it reaches no more IRelationFields by following the default fields).
         *   
         * The Adapter can have a property called SelectedPath which does this behaviour - both setting (and updating ComboSource), and getting.
         * Conditions: 
         *   1) FieldPath.Count == ComboSource.Count is always true
         *   2) ComboSource[i].Contains(FieldPath[i]) is always true
         *   3) No consumer should directly modify ComboSource, it should always be managed by the Adapter
         *   
        */

        /// <summary>
        /// Gets the BindingList for use with the combos.  The outer list represents each combo, the inner list represents the combo data source.
        /// </summary>
        public BindingList<BindingList<IField>> ComboSource
        {
            get;
            private set;
        }

        public FieldPath SelectedPath
        {
            get { return _path; }
            set
            {
                // check old path to new to see what new ComboSources we need to generate
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
                    ComboSource.Add(new BindingList<IField>(_pathFactory.GetFields(_path[i].Subject).Convert<FieldPath, IField>(p => p[0])));
            }
        }
        private FieldPath _path;

        private IFieldPathFactory _pathFactory;
        public FieldPathComboAdapter(IFieldPathFactory pathFactory)
        {
            _pathFactory = pathFactory;
            ComboSource = new BindingList<BindingList<IField>>();
        }
    }
}
