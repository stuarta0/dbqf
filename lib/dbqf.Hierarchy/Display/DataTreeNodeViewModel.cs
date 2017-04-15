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
    public class DataTreeNodeViewModel : TreeNodeViewModel
    {
        public DataTreeNodeViewModel(ITemplateTreeNode template, TreeNodeViewModel parent, bool lazyLoadChildren)
            : base(parent, lazyLoadChildren)
        {
            TemplateNode = template;
        }
        
        /// <summary>
        /// Gets the template node that generated this node.
        /// </summary>
        public ITemplateTreeNode TemplateNode { get; protected set; }

        /// <summary>
        /// Loads children by querying the related TemplateNode's children
        /// </summary>
        protected override void LoadChildren()
        {
            foreach (var childTemplate in TemplateNode)
            {
                foreach (var node in childTemplate.Load(this))
                    Children.Add(node);
            }
        }
    }
}
