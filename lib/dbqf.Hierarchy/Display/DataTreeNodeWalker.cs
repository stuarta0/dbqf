using dbqf.Criterion;
using dbqf.Hierarchy.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace dbqf.Hierarchy.Display
{
    public class DataTreeNodeWalker
    {
        public DataTreeNodeWalker(IDataSource source, DataTreeNodeViewModel root)
        {
            _source = source;
            _root = root;
        }
        private readonly IDataSource _source;
        private readonly DataTreeNodeViewModel _root;

        [DebuggerDisplay("{Template}: {Id}")]
        private class PathNode
        {
            public ITemplateTreeNode Template;
            public object Id;
            public PathNode(ITemplateTreeNode template)
            {
                Template = template;
            }
        }

        private IFieldPath GetPath(DataColumn column)
        {
            if (column.ExtendedProperties.ContainsKey("FieldPath"))
                return column.ExtendedProperties["FieldPath"] as IFieldPath;
            return null;
        }

        public virtual DataTreeNodeViewModel ExpandTo(SubjectTemplateTreeNode target, object id)
        {
            return Walk(target, id, true);
        }

        /// <summary>
        /// Find will walk the tree of nodes looking for the target. Absence only guarantees that the node isn't currently loaded in the tree, not whether it might be in future.
        /// </summary>
        public virtual DataTreeNodeViewModel Find(SubjectTemplateTreeNode target, object id)
        {
            return Walk(target, id, false);
        }

        private DataTreeNodeViewModel Walk(SubjectTemplateTreeNode target, object id, bool expand)
        {
            // target "track" gives [ "artist", "album", "track" ]
            var path = new List<PathNode>();
            var curT = (ITemplateTreeNode)target;
            while (curT != null)
            {
                path.Insert(0, new PathNode(curT));
                curT = curT.Parent;
            }

            // get the id's of all nodes along the path to the target
            var data = _source.GetData(target.Subject,
                path
                    .FindAll(t => t.Template is SubjectTemplateTreeNode && ((SubjectTemplateTreeNode)t.Template).Subject != null)
                    .Select<PathNode, IFieldPath>(t =>
                        FieldPath.FromDefault(((SubjectTemplateTreeNode)t.Template).Subject.IdField))
                    .ToList(),
                new dbqf.Sql.Criterion.SqlSimpleParameter(
                    target.Subject.IdField, "=", id));

            // drop out if we were unable to find data relating to the requested node
            if (data.Rows.Count == 0)
                return null;

            // combine the hierarchy and ids together, noting templates aren't always based on a dbqf Subject
            var j = 0;
            for (int i = 0; i < path.Count && j < data.Columns.Count; i++)
            {
                var subjectTemplate = path[i].Template as SubjectTemplateTreeNode;
                if (subjectTemplate != null && subjectTemplate.Subject == GetPath(data.Columns[j])?.Last.Subject)
                {
                    path[i].Id = data.Rows[0][j];
                    j++;
                }
            }

            // ensure we're starting with loaded children
            var cur = _root;
            if (expand)
                cur.IsExpanded = true;
            for (int i = 1; i < path.Count; i++)
            {
                // we reached a node that hasn't had it's children loaded yet so we're not going to find the target
                if (!expand && cur.HasDummyChild)
                    return null;
                
                var found = false;
                for (int c = 0; !found && c < cur.Children.Count; c++)
                {
                    var dvm = cur.Children[c] as DataTreeNodeViewModel;
                    if (dvm != null)
                    {
                        var subjectTemplate = dvm.TemplateNode as SubjectTemplateTreeNode;
                        if (subjectTemplate != null)
                        {
                            // if this is a grouped subject, iterate the groups until the leaf target is found
                            if (subjectTemplate is GroupedTemplateTreeNode)
                            {
                                // initialise variables to handle traversing grouped nodes
                                var children = new List<TreeNodeViewModel>() { dvm };
                                var counter = 0;
                                while (!found && counter < children.Count)
                                {
                                    var child = children[counter] as DataTreeNodeViewModel;

                                    // make sure we don't recurse off the end of the scope of this node
                                    if (child?.TemplateNode != path[i].Template)
                                        break;

                                    if (child is GroupedTreeNodeViewModel)
                                    {
                                        if (((GroupedTreeNodeViewModel)child).Ids.Contains(path[i].Id))
                                        {
                                            child.IsExpanded = true;
                                            children = new List<TreeNodeViewModel>(child.Children);
                                            counter = 0;
                                        }
                                        else
                                            counter++;
                                    }
                                    else if (child.Id?.Equals(path[i].Id) == true)
                                    {
                                        cur = child;
                                        found = true;
                                    }
                                }
                            }
                            else if (dvm.Id?.Equals(path[i].Id) == true)
                            {
                                // if this node is derived from a SubjectTemplateTreeNode then 
                                // look for the node that contains the matching id value at this
                                // level in the tree
                                cur = dvm;
                                found = true;
                            }
                        }
                        else if (dvm.TemplateNode == path[i].Template)
                        {
                            cur = dvm;
                            found = true;
                        }
                    }
                }
                
                // if we couldn't find the node within the children collection then it must be filtered out or not present in the data
                if (!found || cur == null)
                    break;
                else if (i == path.Count - 1)
                    return cur;
                else if (expand)
                    cur.IsExpanded = true;
            }

            // unsuccessfully tried to find the target node
            return null;
        }
    }
}
