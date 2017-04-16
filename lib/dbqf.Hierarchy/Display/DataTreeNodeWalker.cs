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
            for(int i = 0; i < path.Count && j < data.Columns.Count; i++)
            {
                var subjectTemplate = path[i].Template as SubjectTemplateTreeNode;
                if (subjectTemplate != null && subjectTemplate.Subject == GetPath(data.Columns[j])?.Last.Subject)
                {
                    path[i].Id = data.Rows[0][j];
                    j++;
                }
            }

            _root.IsExpanded = true;
            var cur = _root;
            for (int i = 1; i < path.Count; i++)
            {
                cur = cur.Children.Find(vm =>
                {
                    var dvm = vm as DataTreeNodeViewModel;
                    if (dvm != null)
                    {
                        var subjectTemplate = dvm.TemplateNode as SubjectTemplateTreeNode;
                        if (subjectTemplate != null)
                        {
                            // if this node is derived from a SubjectTemplateTreeNode then 
                            // look for the node that contains the matching id value at this
                            // level in the tree
                            var key = subjectTemplate.Subject.IdField.SourceName;
                            if (dvm.Data.ContainsKey(key))
                                return dvm.Data[key].Equals(path[i].Id);
                        }
                        else
                        {
                            // if it's not a SubjectTemplateTreeNode then the template will 
                            // match the child 1:1 (a single node is created for these templates)
                            // so just find a child whose template matches this part of the path
                            return dvm.TemplateNode == path[i].Template;
                        }
                    }
                    return false;
                }) as DataTreeNodeViewModel;

                // if we couldn't find the node within the children collection then it must be filtered out or not present in the data
                if (cur == null)
                    break;
                else if (i == path.Count - 1)
                    return cur;
                else
                    cur.IsExpanded = true;
            }

            // unsuccessfully tried to expand to the given id
            return null;
        }
    }
}
