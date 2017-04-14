using System;
using System.Collections.Generic;
using System.Text;
using dbqf.Configuration;
using System.Diagnostics;
using dbqf.Criterion;
using System.Collections;
using dbqf.Hierarchy.Display;
using dbqf.Sql;
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
        }
        readonly IDataSource _source;

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
        /// Load nodes for this template node.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataTreeNodeViewModel> Load(DataTreeNodeViewModel parent)
        {
            var fields = new List<IFieldPath>(GetPlaceholders(Text).Values);
            fields.Insert(0, FieldPath.FromDefault(Subject.IdField));
            
            var where = new dbqf.Sql.Criterion.SqlConjunction();
            var curParent = parent;
            for (int i = 0; i < SearchParameterLevels && curParent != null; i++)
            {
                var template = curParent.TemplateNode as SubjectTemplateTreeNode;
                if (template != null)
                {
                    where.Add(new dbqf.Sql.Criterion.SqlSimpleParameter(
                        template.Subject.IdField, "=",
                        curParent.Data[template.Subject.IdField.SourceName]));
                }

                if (curParent.TemplateNode.Parameters != null)
                    where.Add(curParent.TemplateNode.Parameters);

                curParent = parent.Parent as DataTreeNodeViewModel;
            }

            var data = _source.GetData(Subject, fields, where);

            foreach (DataRow row in data.Rows)
            {
                var node = new DataTreeNodeViewModel(this, parent, true);
                foreach (DataColumn col in data.Columns)
                {
                    // result.Columns[i].ExtendedProperties["FieldPath"] = IFieldPath
                    node.Data.Add(GetFieldPathPlaceholder((IFieldPath)col.ExtendedProperties["FieldPath"]), row[col]);
                }
                node.Text = ReplacePlaceholders(Text, node.Data);

                yield return node;
            }
        }

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


        // TODO: re-implement some of the features below after initial development

        ///// <summary>
        ///// Gets or sets the additional fields that will be retrieved when a data node is created from this template.  Note: fields already specified in the Text property will be available.
        ///// </summary>
        //[XmlArray]
        //public List<string> AdditionalFields
        //{
        //	get { return _additional; }
        //	set { _additional = value; }
        //}
        //private List<string> _additional;

        ///// <summary>
        ///// Gets or sets the fields that will be used to create hierarchy based on grouping of data.  This is irrelevant for static nodes.
        ///// </summary>
        //[XmlArray]
        //public List<SortField> GroupBy
        //{
        //	get { return _groups; }
        //	set { _groups = value; }
        //}
        //private List<SortField> _groups;

        ///// <summary>
        ///// Gets or sets the list of fields to sort the data by.
        ///// </summary>
        //[XmlArray]
        //public List<SortField> SortBy
        //{
        //    get { return _sortBy; }
        //    set { _sortBy = value; }
        //}
        //private List<SortField> _sortBy;

        ///// <summary>
        ///// The queryer object to use when getting child nodes for this node and which contains the configuration with the associated subject.
        ///// </summary>
        //[XmlIgnore]
        //public Queryer Queryer
        //{
        //    get { return _queryer; }
        //    set { _queryer = value; }
        //}
        //private Queryer _queryer;

        ///// <summary>
        ///// Should be called after deserialisation on the root node to ensure all objects have the correct references.
        ///// </summary>
        //public void Initialise(Queryer queryer)
        //{
        //    Initialise(queryer, this);
        //}

        //protected void Initialise(Queryer queryer, TemplateNode cur)
        //{
        //    // resolve all subjects and sorted fields through the queryer's configuration
        //    // bind all parent properties based on the Children collections

        //    cur.Queryer = queryer;

        //    if (cur.SubjectID > 0)
        //        cur.Subject = queryer.Configuration.GetSubject(cur.SubjectID);

        //    if (cur.CustomParameters != null)
        //        cur.CustomParameters.InitialiseDeserialization(queryer);

        //    foreach (var child in cur.Children)
        //    {
        //        child.Parent = this;
        //        Initialise(queryer, child);
        //    }
        //}
    }
}
