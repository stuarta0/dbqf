using System;
using System.Collections.Generic;
using System.Text;
using dbqf.Configuration;
using dbqf.Criterion;

namespace dbqf.Hierarchy
{
    /// <summary>
    /// Represents a node in a tree hierarchy that is derived from data in the database.
    /// </summary>
    public class DataNode
    {
  //      public IParameterBuilderFactory Builder { get; set; }

  //      /// <summary>
  //      /// Indicates which node in the template this data node came from.  Used when determining how to load children.
  //      /// NOTE: This property must be assigned - all code relies on this.
  //      /// </summary>
  //      public SubjectTemplateTreeNode DerivedFrom
  //      {
  //          get { return _derivedFrom; }
  //          set { _derivedFrom = value; }
  //      }
  //      private SubjectTemplateTreeNode _derivedFrom;

  //      /// <summary>
  //      /// The ID of this node in the DerivedFrom.Subject.  If DerivedFrom.IsStatic this is ignored.
  //      /// </summary>
  //      public object ID
  //      {
  //          get { return _id; }
  //          set { _id = value; }
  //      }
  //      private object _id;

  //      /// <summary>
  //      /// Used for nodes that are derived from templates with grouping (when not based on a grouping this value will be -1).  Use DerivedFrom.Groups[this.GroupIndex] to determine the group this node was based on.
  //      /// </summary>
  //      public int GroupIndex
  //      {
  //          get { return _gIdx; }
  //          set { _gIdx = value; }
  //      }
  //      private int _gIdx;

  //      /// <summary>
  //      /// Used when grouping to provide hierarchy of result nodes.
  //      /// </summary>
  //      public List<DataNode> Children
  //      {
  //          get { return _children; }
  //          set { _children = value; }
  //      }
  //      private List<DataNode> _children;

  //      /// <summary>
  //      /// The text for this data node.  Usually generated from DerivedFrom.Text placeholders.
  //      /// </summary>
  //      public virtual string Text
  //      {
  //          get { return _text; }
  //          set { _text = value; }
  //      }
  //      private string _text;

  //      /// <summary>
  //      /// The parent data node for this data node.
  //      /// </summary>
  //      public DataNode Parent
  //      {
  //          get { return _parent; }
  //          set { _parent = value; }
  //      }
  //      private DataNode _parent;


  //      /// <summary>
  //      /// Gets or sets whether this data node matched any additional search parameters.
  //      /// </summary>
  //      public bool IsMatch
  //      {
  //          get { return _match; }
  //          set { _match = value; }
  //      }
  //      private bool _match;
		
		///// <summary>
		///// Gets or sets a dictionary of additional data that can be explicitly loaded for each node.
		///// </summary>
		//public Dictionary<IField, object> Data
		//{
		//	get { return _data; }
		//	set { _data = value; }
		//}
		//private Dictionary<IField, object> _data;

  //      public DataNode()
  //      {
  //          GroupIndex = -1;
  //          Children = new List<DataNode>();
  //      }

  //      /// <summary>
  //      /// Gets all children for this node.
  //      /// </summary>
  //      /// <returns></returns>
  //      public List<DataNode> GetChildren()
  //      {
  //          return GetChildren(null);
  //      }

  //      /// <summary>
  //      /// Gets all children for this node limited by the given additional parameters.
  //      /// </summary>
  //      /// <param name="additionalParameters">Any additional parameters used to narrow the children returned.</param>
  //      /// <returns></returns>
  //      public List<DataNode> GetChildren(IParameter additionalParameters)
  //      {
  //          return GetChildren(additionalParameters, false);
  //      }

  //      /// <summary>
  //      /// Gets all children for this node.  If getAll = true, all children will be returned with IsMatch = true for those that also match additional parameters.
  //      /// </summary>
  //      /// <param name="additionalParameters">Any additional parameters used to narrow the children returned.</param>
  //      /// <param name="getAll">If true, sets IsMatch = true for those nodes that also match additional parameters.  If false, only returns children that also match additional parameters.</param>
  //      /// <returns></returns>
  //      public List<DataNode> GetChildren(IParameter additionalParameters, bool getAll)
  //      {
  //          //TODO: this method uses explicit casts to internal concrete classes for Subject and Fields

  //          List<DataNode> children = new List<DataNode>();

  //          // loop through the children template nodes and figure out what children we need to load
  //          foreach (SubjectTemplateTreeNode currentTemplate in DerivedFrom.Children)
  //          {
  //              if (currentTemplate.IsStatic)
  //              {
  //                  // static nodes are just created directly from template
  //                  DataNode node = new DataNode();
  //                  node.DerivedFrom = currentTemplate;
  //                  node.Parent = this;
  //                  node.ID = null;
  //                  node.Text = currentTemplate.Text;

  //                  // static nodes inherit match from parent
  //                  node.IsMatch = this.IsMatch;

  //                  children.Add(node);
  //              }
  //              else
  //              {
  //                  DataNode currentNode = this;
  //                  List<SortField> groups = (currentTemplate.GroupBy ?? new List<SortField>());

  //                  // start with the given additionalParameters which will just be combined with any other parameters from parent template nodes
  //                  // this allows for further filtering at any level of the tree if necessary
  //                  var p = Builder.Conjunction();

  //                  // gather parameters to narrow results for this child
  //                  // loop condition has additional check to ensure we don't traverse off the top of the tree
  //                  for (int level = 0; level < currentTemplate.SearchParameterLevels && currentNode != null; level++)
  //                  {
  //                      // only traverse up non-grouped nodes in the hierarchy
  //                      if (currentNode.GroupIndex == -1)
  //                      {
  //                          var tempParam = currentNode.GetParameters();
  //                          if (tempParam != null)
  //                              p.Add(tempParam);
  //                      }

  //                      currentNode = currentNode.Parent;
  //                  }

  //                  // keep reference to parameters that will give us a wider search in case we need to get all results later
  //                  IParameter widerParms = p;

  //                  // now narrow the normal parameters to get fewer children
  //                  if (additionalParameters != null)
  //                      p.Add(additionalParameters);

  //                  // this is what makes the whole process possible - get all children using search parameters
  //                  //TODO
  //                  List<object> results = null; // currentTemplate.Queryer.Search(currentTemplate.Subject, p);

  //                  // initially set allResults to results, but if we're getting all children update it with those instead
  //                  List<object> allResults = results;
  //                  if (getAll && p != widerParms)
  //                      //TODO
  //                      allResults = null; // currentTemplate.Queryer.Search(currentTemplate.Subject, widerParms);

  //                  var fields = new List<IField>();
  //                  fields.AddRange(ItemData.GetPlaceholders(currentTemplate.Subject, currentTemplate.Text).Values);

  //                  if (currentTemplate.AdditionalFields != null)
		//			    foreach (var f in currentTemplate.AdditionalFields)
		//				    fields.Add (currentTemplate.Subject[f]);

  //                  foreach (var f in groups)
  //                      fields.Add (currentTemplate.Subject[f.FieldSourceName]);


  //                  // grouped fields are sorted first, then by SortBy fields
  //                  var sort = new List<SortField>(groups);
  //                  sort.AddRange(currentTemplate.SortBy);

  //                  //TODO
  //                  List<ItemData> data = null; // currentTemplate.Queryer.GetSortedData(currentTemplate.Subject, allResults, fields, sort);
  //                  children = AddNodes(currentTemplate, data);
  //              }
  //          } // gather children for the next sibling of currentTemplate

  //          return children;
  //      }

  //      private List<DataNode> AddNodes(SubjectTemplateTreeNode template, List<ItemData> data)
  //      {
  //          var dummy = new DataNode();
  //          foreach (var d in data)
  //              AddNode(dummy, 0, template, d);

  //          foreach (var c in dummy.Children)
  //              c.Parent = this;

  //          return dummy.Children;
  //      }

  //      /// <summary>
  //      /// Add node to hierarchy where root collection are nodes relating to groups[0].  If groups is empty, all nodes are added to root collection.
  //      /// </summary>
  //      /// <param name="parent"></param>
  //      /// <param name="groups"></param>
  //      /// <param name="currentGroup"></param>
  //      /// <param name="node"></param>
  //      private void AddNode(DataNode parent, int currentGroup, SubjectTemplateTreeNode template, ItemData item)
  //      {
  //          if (currentGroup >= template.GroupBy.Count)
  //          {
  //              // add node to bottom level 
  //              DataNode node = new DataNode();
  //              node.DerivedFrom = template;
  //              node.Parent = parent;
  //              parent.Children.Add(node);
  //              node.ID = item.Id;
		//	    node.Data = item.Data;
  //              node.Text = item.ReplacePlaceholders (template.Text);
  //              //node.IsMatch = results.Contains(item.Id); // TODO
  //          }
  //          else
  //          {
  //              // add group node if it doesn't already exist and recurse
  //              var field = item.Subject[template.GroupBy[currentGroup].FieldSourceName];
  //              var format = (!String.IsNullOrEmpty(field.DisplayFormat) ? String.Concat("{0:", field.DisplayFormat, "}") : "{0}");
                
  //              var groupName = String.Format(format, (item.Data[field] ?? ""));
  //              var groupNode = parent.Children.Find(n => n.Text.Equals(groupName));
  //              if (groupNode == null)
  //              {
  //                  groupNode = new DataNode();
  //                  groupNode.DerivedFrom = template;
  //                  groupNode.Parent = parent;
  //                  parent.Children.Add(groupNode);
  //                  groupNode.GroupIndex = currentGroup;
  //                  //node.ID = item.Id;
  //                  //node.Data = item.Data;
  //                  groupNode.Text = groupName;
  //              }

  //              AddNode(groupNode, currentGroup + 1, template, item);
  //          }
  //      }

  //      protected IParameter GetParameters()
  //      {
  //          IParameter p = DerivedFrom.CustomParameters;
  //          if (!DerivedFrom.IsStatic)
  //          {
  //              // parameters for a node will be where the Subject at this level is filtered by ID = this node's ID.
  //              // Nodes below this one should filter correctly with this parameter if the configuration has been set up correctly.
  //              var p2 = new SimpleParameter(DerivedFrom.Subject.IdField, "=", ID);

  //              // we also want to combine any custom parameters that this node might have
  //              if (p != null)
  //              {
  //                  p = Builder.Conjunction(p, p2);
  //              }
  //              else
  //                  p = p2;
  //          }

  //          return p;
  //      }

  //      public override string ToString()
  //      {
  //          return Text;
  //      }
    }
}
