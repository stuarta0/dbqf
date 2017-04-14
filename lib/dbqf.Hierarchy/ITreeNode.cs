using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Hierarchy
{
    public interface ITreeNode
    {
        /// <summary>
        /// Gets the template node that generated this node.
        /// </summary>
        ITemplateTreeNode DerivedFrom { get; }

        ITreeNode Parent { get; }
        string Text { get; }

        // TODO: determine if this should be a function, property, or IList interface for the ITreeNode interface.
        IList<ITreeNode> GetChildren();
    }
}
