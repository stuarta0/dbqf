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
            set { _parent = value; }
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

        // TODO: implemented consistency so Parent properties get set correctly when adding or removing from children
        #region IList<ITemplateNode>

        public ITemplateTreeNode this[int index]
        {
            get
            {
                return Children[index];
            }
            set
            {
                Children[index] = value;
            }
        }

        public int Count => Children.Count;
        public bool IsReadOnly => Children.IsReadOnly;
        public int IndexOf(ITemplateTreeNode item) => Children.IndexOf(item);
        public void Insert(int index, ITemplateTreeNode item) => Children.Insert(index, item);
        public void RemoveAt(int index) => Children.RemoveAt(index);
        public void Add(ITemplateTreeNode item) => Children.Add(item);
        public void Clear() => Children.Clear();
        public bool Contains(ITemplateTreeNode item) => Children.Contains(item);
        public void CopyTo(ITemplateTreeNode[] array, int arrayIndex) => Children.CopyTo(array, arrayIndex);
        public bool Remove(ITemplateTreeNode item) => Children.Remove(item);
        public IEnumerator<ITemplateTreeNode> GetEnumerator() => Children.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Children.GetEnumerator();

        #endregion

    }
}
