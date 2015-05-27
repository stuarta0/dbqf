using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Standalone.WPF.Controls
{
    [ImplementPropertyChanged]
    public class RetrieveFieldsViewAdapter
    {
        public bool UseFields { get; set; }

        /// <summary>
        /// Gets the collection of user-defined field paths that should be retrieved.
        /// </summary>
        public ObservableCollection<FieldPath> Fields { get; set; }

        public ObservableCollection<Node> Nodes { get; set; }

        public IFieldPathFactory PathFactory { get; private set; }
        private IConfiguration _configuration;
        public RetrieveFieldsViewAdapter(IConfiguration configuration, IFieldPathFactory pathFactory)
        {
            _configuration = configuration;
            PathFactory = pathFactory;
            Fields = new ObservableCollection<FieldPath>();
            Fields.CollectionChanged += Fields_CollectionChanged;
            Nodes = new ObservableCollection<Node>(_configuration.Convert<ISubject, SubjectNode>(s => new SubjectNode(s, PathFactory)));
        }

        void Fields_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Fields.Count > 0)
                UseFields = true;
            else
                UseFields = false;
        }

        #region Node class definitions

        public class Node
        {
            public Node Parent { get; set; }
            public virtual string Text { get; set; }
            public virtual bool HasChildren { get; set; }
            public virtual ObservableCollection<Node> Children { get; set; }

            public Node()
            {
                HasChildren = false;
            }
        }
        public class SubjectNode : Node 
        { 
            public ISubject Subject { get; private set; }
            public override string Text
            {
                get { return Subject.DisplayName; }
                set { base.Text = value; }
            }

            private IFieldPathFactory _factory;
            public SubjectNode(ISubject subject, IFieldPathFactory factory)
                : base()
            {
                Subject = subject;
                HasChildren = true;
                _factory = factory;
            }

            public override ObservableCollection<Node> Children
            {
                get
                {
                    if (base.Children == null)
                    {
                        base.Children = new ObservableCollection<Node>();
                        foreach (var f in _factory.GetFields(Subject))
                            base.Children.Add(new FieldNode(f[0], _factory) { Parent = this });
                    }
                    return base.Children;
                }
            }
        }
        public class FieldNode : Node 
        { 
            public IField Field { get; private set; }
            public override string Text
            {
                get { return String.Concat(Field.DisplayName, Field is IRelationField ? String.Concat(" (", ((IRelationField)Field).RelatedSubject.DisplayName, ")") : string.Empty); }
                set { base.Text = value; }
            }

            private IFieldPathFactory _factory;
            public FieldNode(IField field, IFieldPathFactory factory)
                : base()
            {
                Field = field;
                HasChildren = field is IRelationField;
                _factory = factory;
            }

            public override ObservableCollection<Node> Children
            {
                get
                {
                    if (base.Children == null)
                    {
                        base.Children = new ObservableCollection<Node>();
                        if (Field is IRelationField)
                            foreach (var f in _factory.GetFields(((IRelationField)Field).RelatedSubject))
                                base.Children.Add(new FieldNode(f[0], _factory) { Parent = this });
                    }
                    return base.Children;
                }
            }
        }

        #endregion

        #region Node handling

        public IEnumerable<FieldPath> Add(Node n)
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
                var fields = PathFactory.GetFields(((IRelationField)((FieldNode)n).Field).RelatedSubject);
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
                return new FieldPath[] { path };
            }
        }

        #endregion
    }
}
