using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dbqf.Hierarchy.Display
{
    public class SubjectTreeNodeViewModel : TreeNodeViewModel
    {
        public SubjectTreeNodeViewModel()
            : base(null, true)
        {
        }

        public SubjectTreeNodeViewModel(TreeNodeViewModel parent, bool lazyLoadChildren)
            : base(parent, lazyLoadChildren)
        {
        }

        protected override void LoadChildren()
        {            
            foreach (var childTemplate in TemplateNode)
            {
                foreach (var node in childTemplate.Load(this))
                    Children.Add(node);
            }
        }
        
        /// <summary>
        /// Gets the template node that generated this node.
        /// </summary>
        ITemplateTreeNode TemplateNode { get; }
    }
}
