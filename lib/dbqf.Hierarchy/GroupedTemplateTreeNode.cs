using dbqf.Hierarchy.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dbqf.Hierarchy.Display;
using dbqf.Hierarchy.Events;
using System.Data;
using dbqf.Criterion;

namespace dbqf.Hierarchy
{
    public class GroupedTemplateTreeNode : SubjectTemplateTreeNode
    {
        public GroupedTemplateTreeNode(IDataSource source)
            : base(source)
        {
            _groups = new List<GroupedField>();
        }

        /// <summary>
        /// Gets or sets the fields that will be used to create hierarchy based on grouping of data.  This is irrelevant for static nodes.
        /// </summary>
        public IList<GroupedField> GroupBy
        {
            get { return _groups; }
        }
        private IList<GroupedField> _groups;

        /// <summary>
        /// Prepare query for execution including order by fields.
        /// </summary>
        protected override DataSourceLoadEventArgs PrepareQuery(DataTreeNodeViewModel parent)
        {
            var args = base.PrepareQuery(parent);

            // inject group fields into the start of the sort order and into the field list
            for (int i = 0; i < GroupBy.Count; i++)
            {
                if (!args.Fields.Contains(GroupBy[i].FieldPath))
                    args.Fields.Add(GroupBy[i].FieldPath);
                args.OrderBy.Insert(i, GroupBy[i]);
            }

            return args;
        }

        /// <summary>
        /// Load nodes for this template node.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataTreeNodeViewModel> Load(DataTreeNodeViewModel parent)
        {
            // allow interception of what we'll be requesting from the data source
            var args = OnDataSourceLoading(PrepareQuery(parent));
            if (args.Cancel)
                yield break;
            var data = _source.GetData(args.Target, args.Fields, args.Where, args.OrderBy);

            // precompile keys from field paths in columns
            var columns = new Dictionary<IFieldPath, Column>();
            foreach (DataColumn col in data.Columns)
            {
                // result.Columns[i].ExtendedProperties["FieldPath"] = IFieldPath
                var path = (IFieldPath)col.ExtendedProperties["FieldPath"];
                columns.Add(path, new Column() { DataColumn = col, Key = GetFieldPathPlaceholder(path) });
            }
            var idColumn = columns[FieldPath.FromDefault(Subject.IdField)];

            // precompile groups for tracking current node hierarchy
            var groups = new List<Group>();
            foreach (var f in GroupBy)
                groups.Add(new Group() { Field = f });

            var visited = new List<object>();
            foreach (DataRow row in data.Rows)
            {
                // process the current group hierarchy
                var newGroup = false;
                for (int i = 0; i < groups.Count; i++)
                {
                    var group = groups[i];
                    var name = row[columns[group.Field.FieldPath].DataColumn]?.ToString();
                    name = String.IsNullOrWhiteSpace(name) ? group.Field.EmptyPlaceholder : name;

                    if (newGroup || group.CurNode == null || group.CurNode.Text?.Equals(name) == false)
                    {
                        newGroup = true;
                        group.CurNode = new GroupedTreeNodeViewModel(this, i > 0 ? groups[i - 1].CurNode : parent, false)
                        {
                            Text = name,
                            Group = group.Field
                        };

                        // yield the root grouped node for adding to the parent child collection, otherwise add it to the previous group
                        if (i == 0)
                            yield return group.CurNode;
                        else
                            groups[i - 1].CurNode.Children.Add(group.CurNode); 
                    }

                    // add the data for this result's ID to assist in traversal
                    group.CurNode.Ids.Add(row[idColumn.DataColumn]);
                }

                // prepare leaf node of grouping
                var node = base.CreateNode(row, columns.Values, groups.Count > 0 ? groups[groups.Count - 1].CurNode : parent, args.Data);
                
                // ensure that we don't duplicate rows in the case of where criteria or requested fields returning multiple rows
                if (visited.Contains(node.Data[IdKey]))
                    continue;
                visited.Add(node.Data[IdKey]);

                // if we didn't have any groups, just return this base node, otherwise add it to the last group
                if (groups.Count == 0)
                    yield return node;
                else
                    groups[groups.Count - 1].CurNode.Children.Add(node);
            }
        }

        public GroupedTemplateTreeNode AddGroupBy(params GroupedField[] fields)
        {
            foreach (var f in fields)
                GroupBy.Add(f);

            return this;
        }

        private class Group
        {
            public GroupedField Field;
            public GroupedTreeNodeViewModel CurNode;
        }
    }
}
