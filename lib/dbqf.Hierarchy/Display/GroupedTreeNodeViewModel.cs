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
        }
        
        /// <summary>
        /// Gets the template group field that generated this node.
        /// </summary>
        public OrderedField Group { get; set; }
    }
}
