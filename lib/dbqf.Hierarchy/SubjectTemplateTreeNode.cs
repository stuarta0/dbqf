using System;
using System.Collections.Generic;
using System.Text;
using dbqf.Configuration;
using System.Diagnostics;
using dbqf.Criterion;
using System.Collections;
using dbqf.Hierarchy.Display;
using System.Text.RegularExpressions;
using dbqf.Hierarchy.Data;
using System.Data;

namespace dbqf.Hierarchy
{
    /// <summary>
    /// Represents a template from which to retrieve the actual data nodes of a tree hierarchy.
    /// </summary>
    [DebuggerDisplay("{Subject}: {Text}")]
    public class SubjectTemplateTreeNode : TemplateTreeNode
    {
        protected static readonly Regex PLACEHOLDER_RE = new Regex(@"\{[^\}]+\}");

        public SubjectTemplateTreeNode(IDataSource source)
            : base()
        {
            _source = source;
            _additional = new List<IFieldPath>();
            _orderBy = new List<OrderedField>();
        }

        #region Properties

        protected readonly IDataSource _source;

        /// <summary>
        /// Occurs prior to loading nodes for this template. Parameters can be modified or the request cancelled by the handler.
        /// This event also bubbles from it's children so it can be handled by registering with the root node.
        /// </summary>
        public event EventHandler<Events.DataSourceLoadEventArgs> DataSourceLoading;
        protected virtual Events.DataSourceLoadEventArgs OnDataSourceLoading(Events.DataSourceLoadEventArgs args)
        {
            DataSourceLoading?.Invoke(this, args);
            return args;
        }

        /// <summary>
        /// The subject of this node.  This defines what type of item to load at this level of the tree.
        /// </summary>
        public ISubject Subject
        {
            get { return _subject; }
            set 
            { 
                _subject = value;
                //SubjectID = (_subject != null ? _subject.ID : 0);
            }
        }
        private ISubject _subject;

        /// <summary>
        /// Gets or sets the template node text that can include tags. e.g. "Product {Number}: {Name}" -> "Product 123: Mountain Bike"
        /// </summary>
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }
        
        /// <summary>
        /// Defines how many levels up the tree this node needs to traverse to gather search parameters to narrow results at this level.
        /// For example, 0 means don't gather parameters at all; 1 means get parameters from the level above this one; 3 means gather parameters from
        /// the level above this one, followed by the level above that, and above that.
        /// </summary>
        public int SearchParameterLevels
        {
            get { return _searchParamLevels; }
            set { _searchParamLevels = value; }
        }
        private int _searchParamLevels;

        /// <summary>
        /// Gets or sets the additional fields that will be retrieved when a data node is created from this template.  Note: fields already specified in the Text property will be available.
        /// </summary>
        public List<IFieldPath> AdditionalFields
        {
            get { return _additional; }
        }
        private List<IFieldPath> _additional;

        /// <summary>
        /// Gets or sets the list of fields to sort the data by.
        /// </summary>
        public List<OrderedField> OrderBy
        {
            get { return _orderBy; }
        }
        private List<OrderedField> _orderBy;

        /// <summary>
        /// Shorthand for calculating the key for accessing the ID in the Data dictionary.
        /// e.g. node.Data[node.Template.Subject.IdField.SourceName] becomes node.Data[node.Template.IdKey]
        /// </summary>
        public string IdKey
        {
            get { return Subject.IdField.SourceName; }
        }

        #endregion

        #region Loading data

        /// <summary>
        /// Compiles target, fields and where criteria, firing the DataSourceLoad event and returns the final args for execution with IDataSource.GetData().
        /// </summary>
        protected virtual Events.DataSourceLoadEventArgs PrepareQuery(DataTreeNodeViewModel parent)
        {
            // Add fields from placeholder text
            var fields = new List<IFieldPath>(GetPlaceholders(Text).Values);

            // Ensure ID field is present
            if (fields.Find(p => p.Last.Equals(Subject.IdField)) == null)
                fields.Insert(0, FieldPath.FromDefault(Subject.IdField));

            // Add additional fields requested by consumer
            foreach (var f in AdditionalFields)
            {
                if (!fields.Contains(f))
                    fields.Add(f);
            }
            
            // compile where clause from parent node
            var where = new dbqf.Sql.Criterion.SqlConjunction();
            var curParent = parent;
            for (int i = 0; i < SearchParameterLevels && curParent != null; i++)
            {
                var template = curParent.TemplateNode as SubjectTemplateTreeNode;
                if (template != null)
                {
                    where.Add(new dbqf.Sql.Criterion.SqlSimpleParameter(
                        template.Subject.IdField, "=",
                        curParent.Data[template.IdKey]));
                }

                if (curParent.TemplateNode.Parameters != null)
                    where.Add(curParent.TemplateNode.Parameters);

                curParent = curParent.Parent as DataTreeNodeViewModel;
            }

            if (where.Count == 0)
                where = null;

            return new Events.DataSourceLoadEventArgs(Subject, fields, (where != null && where.Count == 1 ? where[0] : where), OrderBy);
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
            var columns = new List<Column>();
            foreach (DataColumn col in data.Columns)
            { 
                // result.Columns[i].ExtendedProperties["FieldPath"] = IFieldPath
                columns.Add(new Column()
                {
                    Key = GetFieldPathPlaceholder((IFieldPath)col.ExtendedProperties["FieldPath"]),
                    DataColumn = col
                });
            }

            var visited = new List<object>();
            foreach (DataRow row in data.Rows)
            {
                var node = CreateNode(row, columns, parent, args.Data);

                // ensure that we don't duplicate rows in the case of where criteria or requested fields returning multiple rows
                if (visited.Contains(node.Data[IdKey]))
                    continue;
                visited.Add(node.Data[IdKey]);

                yield return node;
            }
        }

        protected virtual DataTreeNodeViewModel CreateNode(DataRow row, IEnumerable<Column> columns, TreeNodeViewModel parent, Dictionary<string, object> data)
        {
            // prepare leaf node of grouping
            var node = new DataTreeNodeViewModel(this, parent, Children.Count > 0);
            foreach (var col in columns)
                node.Data.Add(col.Key, row[col.DataColumn]);

            // add any provided data from an observer to each node too, overriding returned values
            foreach (var pair in data)
                node.Data.Add(pair.Key, pair.Value);

            node.Id = node.Data.ContainsKey(IdKey) ? node.Data[IdKey] : null;
            node.Text = ReplacePlaceholders(Text, node.Data);
            return node;
        }

        protected struct Column
        {
            public DataColumn DataColumn;
            public string Key;
        }

        #endregion

        #region Placeholder string manipulation

        /// <summary>
        /// Given an IFieldPath, create a placeholder string.
        /// e.g. IFieldPath[ "RelationField1", "Field2" ] returns "RelationField1.Field2"
        /// </summary>
        protected virtual string GetFieldPathPlaceholder(IFieldPath path)
        {
            var sb = new StringBuilder();
            foreach (var part in path)
                sb.Append($"{part.SourceName}.");

            return sb.Length > 0 ? sb.Remove(sb.Length - 1, 1).ToString() : null;
        }

        /// <summary>
        /// Given a placeholder string, extract the field paths from this template's subject.
        /// e.g. "A string with {RelationField1.Field2}" returns { "RelationField1.Field2": IFieldPath[ "RelationField1", "Field2" ] }
        /// </summary>
        protected virtual Dictionary<string, IFieldPath> GetPlaceholders(string placeholderText)
        {
            var paths = new Dictionary<string, IFieldPath>();

            var matches = PLACEHOLDER_RE.Matches(placeholderText);
            foreach (Match m in matches)
            {
                // {field}
                // {relationfield} - determine default field and use that
                // {relationfield.childrelationfield.childfield}

                if (!paths.ContainsKey(m.Value))
                {
                    string key = m.Value.TrimStart('{').TrimEnd('}');
                    string[] parts = key.Split('.');

                    var subject = Subject;
                    var path = new FieldPath();
                    foreach (var part in parts)
                    {
                        var field = subject[part];
                        if (field is IRelationField)
                            subject = ((IRelationField)field).RelatedSubject;
                        else if (field == null)
                            break;

                        path.Add(field);
                    }

                    if (path.Count > 0)
                    {
                        if (path.Last is IRelationField)
                            path.Add(FieldPath.FromDefault(path.Last)[1, null]);

                        paths.Add(key, path);
                    }
                }
            }

            return paths;
        }


        /// <summary>
        /// Replaces the placeholders in the given string with data from this item. The replaced data will honour the relevant field's DisplayFormat too.
        /// e.g. "A string with {RelationField1.Field2}" and data{ "RelationField1.Field2": "db data" } returns "A string with db data"
        /// </summary>
        public string ReplacePlaceholders(string placeholderText, Dictionary<string, object> data)
        {
            // query the placeholder dictionary so we have context for formatting values
            var placeholders = GetPlaceholders(placeholderText);
            return PLACEHOLDER_RE.Replace(placeholderText,
                new MatchEvaluator((m) =>
                {
                    // m.Value contains the field name with brackets
                    string fieldName = m.Value.TrimStart('{').TrimEnd('}');

                    if (placeholders.ContainsKey(fieldName) && data.ContainsKey(fieldName))
                    {
                        var field = placeholders[fieldName];
                        var value = data[fieldName];
                        if (value == null)
                            return String.Empty;
                        else if (!String.IsNullOrEmpty(field.Last.DisplayFormat))
                            return String.Format(String.Concat("{0:", field.Last.DisplayFormat, "}"), value);

                        return value.ToString();
                    }

                    return String.Empty;
                }));
        }

        #endregion

        #region Bubbling event overrides

        private void RegisterNode(ITemplateTreeNode item)
        {
            if (!Contains(item))
            {
                var subjectNode = item as SubjectTemplateTreeNode;
                if (subjectNode != null)
                    subjectNode.DataSourceLoading += SubjectTemplateTreeNode_DataSourceLoad;
            }
        }

        private void UnregisterNode(ITemplateTreeNode item)
        {
            var subjectNode = item as SubjectTemplateTreeNode;
            if (subjectNode != null)
                subjectNode.DataSourceLoading -= SubjectTemplateTreeNode_DataSourceLoad;
        }

        public override void Add(ITemplateTreeNode item)
        {
            RegisterNode(item);
            base.Add(item);
        }

        public override void Insert(int index, ITemplateTreeNode item)
        {
            RegisterNode(item);
            base.Insert(index, item);
        }

        public override bool Remove(ITemplateTreeNode item)
        {
            UnregisterNode(item);
            return base.Remove(item);
        }

        private void SubjectTemplateTreeNode_DataSourceLoad(object sender, Events.DataSourceLoadEventArgs e)
        {
            // bubble the child events up
            DataSourceLoading?.Invoke(sender, e);
        }

        #endregion

        #region Fluent helpers

        public SubjectTemplateTreeNode AddAdditionalField(params IFieldPath[] fields)
        {
            foreach (var f in fields)
                AdditionalFields.Add(f);

            return this;
        }

        public SubjectTemplateTreeNode AddOrderBy(params OrderedField[] fields)
        {
            foreach (var f in fields)
                OrderBy.Add(f);

            return this;
        }

        #endregion

        public override string ToString()
        {
            return $"{Subject?.DisplayName} \"{Text}\"";
        }
    }
}
