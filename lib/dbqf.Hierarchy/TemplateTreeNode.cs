using dbqf.Criterion;
using dbqf.Hierarchy.Display;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace dbqf.Hierarchy
{
    [DebuggerDisplay("{Text}")]
    public class TemplateTreeNode : ITemplateTreeNode
    {
        public TemplateTreeNode()
        {
            Children = new List<ITemplateTreeNode>();
        }

        /// <summary>
        /// Parent node for this template node.  Root level parent will be null.
        /// </summary>
        public virtual ITemplateTreeNode Parent
        {
            get { return _parent; }
            set
            {
                if (_parent == value)
                    return;
                
                if (_parent != null)
                {
                    var parent = _parent;
                    _parent = null;
                    parent.Remove(this);
                }

                _parent = value;

                if (_parent != null)
                    _parent.Add(this);
            }
        }
        private ITemplateTreeNode _parent;

        /// <summary>
        /// Placeholder text to be shown in final ITreeNode.
        /// </summary>
        public virtual string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        private string _text;

        /// <summary>
        /// Additional parameters to use for children nodes to narrow their results.
        /// </summary>
        public virtual IParameter Parameters
        {
            get { return _customParms; }
            set { _customParms = value; }
        }
        private IParameter _customParms;

        /// <summary>
        /// Child template nodes underneath this template node.
        /// </summary>
        protected virtual IList<ITemplateTreeNode> Children
        {
            get { return _children; }
            set { _children = value; }
        }
        private IList<ITemplateTreeNode> _children;

        /// <summary>
        /// Load nodes for this template node.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<DataTreeNodeViewModel> Load(DataTreeNodeViewModel parent)
        {
            yield return new DataTreeNodeViewModel(this, parent, true)
            {
                Text = Text
            };
        }

        /// <summary>
        /// Fluent method to add multiple children and return itself.
        /// </summary>
        public TemplateTreeNode AddChildren(params ITemplateTreeNode[] children)
        {
            foreach (var child in children)
                Add(child);

            return this;
        }
        
        #region IList<ITemplateNode>

        public virtual ITemplateTreeNode this[int index]
        {
            get
            {
                return Children[index];
            }
            set
            {
                // TODO: test
                Children[index].Parent = null; // this will remove it from the collection
                Insert(index, value); // we need to insert the new node at the old node's index
            }
        }

        public virtual int Count => Children.Count;
        public virtual bool IsReadOnly => Children.IsReadOnly;
        public virtual int IndexOf(ITemplateTreeNode item) => Children.IndexOf(item);
        public virtual bool Contains(ITemplateTreeNode item) => Children.Contains(item);
        public virtual void CopyTo(ITemplateTreeNode[] array, int arrayIndex) => Children.CopyTo(array, arrayIndex);
        public virtual IEnumerator<ITemplateTreeNode> GetEnumerator() => Children.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Children.GetEnumerator();
        public virtual void Insert(int index, ITemplateTreeNode item)
        {
            if (!Contains(item))
            {
                Children.Insert(index, item);
                item.Parent = this;
            }
        }
        public virtual void RemoveAt(int index)
        {
            // Setting parent null of child at index will cause the item to be removed via this.Remove(item)
            Children[index].Parent = null;
        }
        public virtual void Add(ITemplateTreeNode item)
        {
            if (!Contains(item))
            {
                Children.Add(item);
                item.Parent = this;
            }
        }
        public virtual void Clear()
        {
            // TODO: ensure behaviour is correct since Children collection may change during call
            foreach (var t in new System.Collections.ObjectModel.ReadOnlyCollection<ITemplateTreeNode>(Children))
                t.Parent = null;
            Children.Clear();
        }
        public virtual bool Remove(ITemplateTreeNode item)
        {
            item.Parent = null;
            return Children.Remove(item);
        }

        #endregion

    }
}
