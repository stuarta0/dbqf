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
            get { return _method; }
            set
            {
                if (_method == value)
                    return;
                _method = value;
                OnPropertyChanged("RetrieveFieldMethod");
                OnPropertyChanged("FieldsEnabled");
            }
        }
        private Method _method;

        /// <summary>
        /// Gets or sets the current path for adding a custom field path.
        /// </summary>
        public FieldPath SelectedPath { get; set; }

        public bool FieldsEnabled
        {
            get { return RetrieveFieldMethod == Method.Custom; }
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
        private IConfiguration _configuration;
        public RetrieveFieldsViewAdapter(IConfiguration configuration, IFieldPathFactory pathFactory)
        {
            _configuration = configuration;
            PathFactory = pathFactory;
            Fields = new BindingList<FieldPath>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }



        public class Node
        {
            public Node Parent { get; set; }
            public string Text { get; set; }
            public bool HasChildren { get; set; }
        }
        public class SubjectNode : Node { public ISubject Subject { get; set; } }
        public class FieldNode : Node { public IField Field { get; set; } }

        public IEnumerable<Node> GetChildren(Node node = null)
        {
            ISubject toExpand = null;
            if (node == null)
                foreach (var s in _configuration)
                    yield return new SubjectNode() { Text = s.DisplayName, HasChildren = true, Subject = s };
            else
            {
                if (node is SubjectNode)
                    toExpand = ((SubjectNode)node).Subject;
                else if (node is FieldNode)
                {
                    if (((FieldNode)node).Field is IRelationField)
                        toExpand = ((IRelationField)((FieldNode)node).Field).RelatedSubject;
                    else
                        yield break; // can't expand non-relation fields
                }

                foreach (var f in PathFactory.GetFields(toExpand))
                    yield return new FieldNode() { Parent = node, Text = f.Description, HasChildren = f[0] is IRelationField, Field = f[0] };
            }
        }

        public void Add(Node n)
        {
            // if node is SubjectNode, add all fields (and defaults for IRelationFields)
            // if node is FieldNode with an IRelationField, add all fields for the related subject (and defaults for IRelationFields)
            // if node is FieldNode with an IField, create a path tracing back to the parent
            
            if (n is SubjectNode)
            {
                foreach (var f in PathFactory.GetFields(((SubjectNode)n).Subject))
                {
                    f.Description = null; // this is bad behaviour from the FieldPathFactory
                    if (!Fields.Contains(f))
                        Fields.Add(f);
                }
            }
            else if (n is FieldNode && ((FieldNode)n).Field is IRelationField)
            {
                foreach (var f in PathFactory.GetFields(((IRelationField)((FieldNode)n).Field).RelatedSubject))
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
            }
        }
    }
}
