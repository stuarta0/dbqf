using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace dbqf.Hierarchy.Display
{
    /// <summary>
    /// A tree node loaded via dbqf functionality.
    /// </summary>
    [DebuggerDisplay("{Text} - {TemplateNode}")]
    public class GroupedTreeNodeViewModel : DataTreeNodeViewModel
    {
        public GroupedTreeNodeViewModel(ITemplateTreeNode template, TreeNodeViewModel parent, bool lazyLoadChildren)
            : base(template, parent, lazyLoadChildren)
        {
            Ids = new List<object>();
        }
        
        /// <summary>
        /// Gets the template group field that generated this node.
        /// </summary>
        public OrderedField Group { get; set; }

        /// <summary>
        /// Gets the ID's of all the DataTreeNodeViewModel contained within this grouped node for traversal.
        /// </summary>
        // TODO: if children are removed below this grouped node, the Ids collection should be updated
        public ICollection<object> Ids { get; private set; }
    }
}
