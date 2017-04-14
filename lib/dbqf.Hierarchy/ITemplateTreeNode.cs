using dbqf.Criterion;
using dbqf.Hierarchy.Display;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Hierarchy
{
    /// <summary>
    /// Represents a template node in a tree, optionally containing child nodes obtained via the IList interface.
    /// This interface is used to define how a tree will be constructed, not the resulting tree itself. See <see cref="dbqf.Hierarchy.ITreeNode"/> for implementation.
    /// </summary>
    public interface ITemplateTreeNode : IList<ITemplateTreeNode>
    {
        /// <summary>
        /// Parent node of this node.
        /// </summary>
        ITemplateTreeNode Parent { get; }

        /// <summary>
        /// Gets the text to display for this node.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Ability to provide additional parameters at this level of the tree.
        /// </summary>
        IParameter Parameters { get; }

        /// <summary>
        /// Load view model nodes for this template node.
        /// </summary>
        /// <returns></returns>
        IEnumerable<DataTreeNodeViewModel> Load(DataTreeNodeViewModel parent);
    }
}
