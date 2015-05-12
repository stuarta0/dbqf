using dbqf.Criterion;
using dbqf.Display;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Forms
{
    public class RetrieveFieldsViewAdapter
    {
        public enum Method
        {
            Predefined = 0,
            Custom = 1
        }

        /// <summary>
        /// Gets or sets the current method for which fields should be retrieved in a result set.
        /// </summary>
        public Method RetrieveFieldMethod
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current path for adding a custom field path.
        /// </summary>
        public FieldPath SelectedPath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the collection of field paths that should be retrieved based on the RetrieveFieldMethod.
        /// </summary>
        public BindingList<FieldPath> Fields
        {
            get
            {
                if (RetrieveFieldMethod == Method.Predefined && SelectedPath != null)
                    return new BindingList<FieldPath>(PathFactory.GetFields(SelectedPath.Root));
                return _fields;
            }
            private set
            {
                _fields = value;
            }
        }
        private BindingList<FieldPath> _fields;

        public IFieldPathFactory PathFactory { get; private set; }
        public RetrieveFieldsViewAdapter(IFieldPathFactory pathFactory)
        {
            PathFactory = pathFactory;
            Fields = new BindingList<FieldPath>();
        }
    }
}
