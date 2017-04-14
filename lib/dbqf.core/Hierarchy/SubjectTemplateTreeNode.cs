using System;
using System.Collections.Generic;
using System.Text;
using dbqf.Configuration;
using System.Diagnostics;
using dbqf.Criterion;
using System.Collections;

namespace dbqf.Hierarchy
{
    /// <summary>
    /// Represents a template from which to retrieve the actual data nodes of a tree hierarchy.
    /// </summary>
    [DebuggerDisplay("{Subject}: {Text}")]
    public class SubjectTemplateTreeNode : TemplateTreeNode
    {
        public SubjectTemplateTreeNode()
            : base()
        {
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
