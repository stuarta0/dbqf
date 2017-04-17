using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace dbqf.Hierarchy.Display
{
    /// <summary>
    /// A generic TreeNode ViewModel that can be bound to a tree control and lazy load it's children.
    /// </summary>
    [DebuggerDisplay("{Text}")]
    public class TreeNodeViewModel : INotifyPropertyChanged
    {
        private TreeNodeViewModel() { }

        public TreeNodeViewModel(TreeNodeViewModel parent, bool lazyLoadChildren)
            : this()
        {
            _parent = parent;

            _children = new ObservableCollection<TreeNodeViewModel>();
            _data = new Dictionary<string, object>();

            _lazyLoadChildren = lazyLoadChildren;
            Reset();
        }
        private bool _lazyLoadChildren = false;
        static readonly TreeNodeViewModel DummyChild = new TreeNodeViewModel();


        /// <summary>
        /// Returns the logical child items of this object.
        /// </summary>
        public ObservableCollection<TreeNodeViewModel> Children
        {
            get { return _children; }
        }
        readonly ObservableCollection<TreeNodeViewModel> _children;

        /// <summary>
        /// Occurs once children have loaded for this node.
        /// </summary>
        public event EventHandler ChildrenLoaded;

        /// <summary>
        /// Returns the parent of this node.
        /// </summary>
        public TreeNodeViewModel Parent
        {
            get { return _parent; }
            protected set { _parent = value; }
        }
        private TreeNodeViewModel _parent;

        /// <summary>
        /// The text to display against this node.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets additional data associated with this node. 
        /// e.g. {Binding Path=Data[FieldName]}
        /// </summary>
        public Dictionary<string, object> Data
        {
            get { return _data; }
        }
        readonly Dictionary<string, object> _data;

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }
        private bool _isSelected;

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (_isExpanded && _parent != null)
                    _parent.IsExpanded = true;

                // Lazy load the child items, if necessary.
                if (_isExpanded && this.HasDummyChild)
                {
                    this.Children.Remove(DummyChild);
                    this.LoadChildren();
                }
            }
        }
        private bool _isExpanded;

        /// <summary>
        /// Returns true if this object's Children have not yet been populated.
        /// </summary>
        public bool HasDummyChild
        {
            get { return this.Children.Count == 1 && this.Children[0] == DummyChild; }
        }

        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        protected virtual void LoadChildren()
        {
            ChildrenLoaded?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Removes subtree under this node and resets lazy loading.
        /// </summary>
        public virtual void Reset()
        {
            IsExpanded = false;
            Children.Clear();
            if (_lazyLoadChildren)
                _children.Add(DummyChild);
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
