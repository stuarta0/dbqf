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




        /// <summary>
        /// Gets the BindingList for use with the combos.  The outer list represents each combo, the inner list represents the combo data source.
        /// </summary>
        public BindingList<BindingList<IField>> ComboSource
        {
            get;
            set;
        }

        public IList<IField> BasePaths 
        {
            get { return ComboSource[0]; }
            set
            {
                // rebuild ComboSource as we've replaced the base paths, select the first field
                ComboSource.Clear();
                ComboSource.Add(new BindingList<IField>(value));
            }
        }

        private FieldPathFactory _pathFactory;
        public FieldPathComboAdapter(FieldPathFactory pathFactory)
        {
            _pathFactory = pathFactory;
        }
    }
}
