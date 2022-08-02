using dbqf.Configuration;
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
    public class RetrieveFieldsViewAdapter : INotifyPropertyChanged
    {
        public bool UseFields
        {
            get { return _use; }
            set
            {
                if (_use == value)
                    return;
                _use = value;
                OnPropertyChanged("UseFields");
            }
        }
        private bool _use;

        /// <summary>
        /// Gets or sets the current path for adding a custom field path.
        /// </summary>
        public IFieldPath SelectedPath { get; set; }

        /// <summary>
        /// Gets the collection of user-defined field paths that should be retrieved.
        /// </summary>
        public BindingList<IFieldPath> Fields { get; set; }

        public IFieldPathFactory PathFactory { get; private set; }
        private IConfiguration _configuration;
        public RetrieveFieldsViewAdapter(IConfiguration configuration, IFieldPathFactory pathFactory)
        {
            _configuration = configuration;
            PathFactory = pathFactory;
            Fields = new BindingList<IFieldPath>();
            Fields.ListChanged += Fields_ListChanged;
        }

        void Fields_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (Fields.Count > 0)
                UseFields = true;
            else
                UseFields = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }


        #region Node class definitions

        public class Node
        {
            public Node Parent { get; set; }
            public virtual string Text { get; set; }
            public virtual bool HasChildren { get; set; }
        }
        public class SubjectNode : Node 
        { 
            public ISubject Subject { get; set; } 
            public override string Text
            {
                get
                {
                    return Subject.DisplayName;
                }
                set
                {
                    base.Text = value;
                }
            }
        }
        public class FieldNode : Node 
        { 
            public IField Field { get; set; }
            public override string Text
            {
                get
                {
                    return String.Concat(Field.DisplayName, Field is IRelationField ? String.Concat(" (", ((IRelationField)Field).RelatedSubject.DisplayName, ")") : string.Empty);
                }
                set
                {
                    base.Text = value;
                }
            }
        }

        #endregion

        #region Node handling

        public IEnumerable<Node> GetChildren(Node node = null)
        {
            //ISubject toExpand = null;
            if (node == null)
                foreach (var s in _configuration)
                    yield return new SubjectNode() { HasChildren = true, Subject = s };
            else
            {
                if (node is SubjectNode)
                {
                    foreach (var f in PathFactory.GetFields(((SubjectNode)node).Subject))
                        yield return new FieldNode() { Parent = node, HasChildren = f[0] is IRelationField, Field = f[0] };
                }
                else if (node is FieldNode && ((FieldNode)node).Field is IRelationField)
                {
                    foreach (var f in PathFactory.GetFields((IRelationField)((FieldNode)node).Field))
                        yield return new FieldNode() { Parent = node, HasChildren = f[0] is IRelationField, Field = f[0] };
                }
                else
                    yield break;
            }
        }

        public IEnumerable<IFieldPath> Add(Node n)
        {
            // if node is SubjectNode, add all fields (and defaults for IRelationFields)
            // if node is FieldNode with an IRelationField, add all fields for the related subject (and defaults for IRelationFields)
            // if node is FieldNode with an IField, create a path tracing back to the parent
            
            if (n is SubjectNode)
            {
                var fields = PathFactory.GetFields(((SubjectNode)n).Subject);
                foreach (var f in fields)
                {
                    f.Description = null; // this is bad behaviour from the FieldPathFactory
                    if (!Fields.Contains(f))
                        Fields.Add(f);
                }
                return fields;
            }
            else if (n is FieldNode && ((FieldNode)n).Field is IRelationField)
            {
                var fields = PathFactory.GetFields((IRelationField)((FieldNode)n).Field);
                foreach (var f in fields)
                {
                    // ensure hierarchy is maintained
                    var parent = n;
                    while (parent != null && parent is FieldNode)
                    {
                        f.Insert(0, ((FieldNode)parent).Field);
                        parent = parent.Parent;
                    }
                    f.Description = null;
                    if (!Fields.Contains(f))
                        Fields.Add(f);
                }
                return fields;
            }
            else
            {
                var path = new FieldPath();
                var parent = n;
                while (parent != null && parent is FieldNode)
                {
                    path.Insert(0, ((FieldNode)parent).Field);
                    parent = parent.Parent;
                }
                if (!Fields.Contains(path))
                    Fields.Add(path);
                return new IFieldPath[] { path };
            }
        }

        #endregion
    }
}
