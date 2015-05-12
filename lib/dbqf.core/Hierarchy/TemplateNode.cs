using System;
using System.Collections.Generic;
using System.Text;
using dbqf.Configuration;
using System.Xml.Serialization;
using System.Diagnostics;
using dbqf.Criterion;

namespace dbqf.Hierarchy
{
    /// <summary>
    /// Represents a template from which to retrieve the actual data nodes of a tree hierarchy.
    /// </summary>
    [DebuggerDisplay("{Subject}: {Text}")]
    public class TemplateNode
    {
        /// <summary>
        /// Parent node for this template node.  Root level parent will be null.
        /// </summary>
        [XmlIgnore]
        public TemplateNode Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        private TemplateNode _parent;
        
        /// <summary>
        /// Used for serialization to resolve the subject later.
        /// </summary>
        [XmlAttribute]
        public int SubjectID
        {
            get { return _subjectID; }
            set { _subjectID = value; }
        }
        private int _subjectID;

        /// <summary>
        /// The subject of this node.  This defines what type of item to load at this level of the tree.
        /// </summary>
        [XmlIgnore]
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
        /// Placeholder text to be populated in DataNode.  e.g "[Number]: [Name]" -> "0012: Specialised Mountain Bike"
        /// </summary>
        [XmlElement]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        private string _text;

        /// <summary>
        /// Image to assign to this node when displaying in the UI.
        /// </summary>
        [XmlElement]
        public string ImageKey
        {
            get { return _imageKey; }
            set { _imageKey = value; }
        }
        private string _imageKey;
		
		/// <summary>
		/// Gets or sets the additional fields that will be retrieved when a data node is created from this template.  Note: fields already specified in the Text property will be available.
		/// </summary>
		[XmlArray]
		public List<string> AdditionalFields
		{
			get { return _additional; }
			set { _additional = value; }
		}
		private List<string> _additional;
		
		/// <summary>
		/// Gets or sets the fields that will be used to create hierarchy based on grouping of data.  This is irrelevant for static nodes.
		/// </summary>
		[XmlArray]
		public List<SortField> GroupBy
		{
			get { return _groups; }
			set { _groups = value; }
		}
		private List<SortField> _groups;

        /// <summary>
        /// Gets or sets the list of fields to sort the data by.
        /// </summary>
        [XmlArray]
        public List<SortField> SortBy
        {
            get { return _sortBy; }
            set { _sortBy = value; }
        }
        private List<SortField> _sortBy;

        /// <summary>
        /// Defines how many levels up the tree this node needs to traverse to gather search parameters to narrow results at this level.
        /// For example, 0 means don't gather parameters at all; 1 means get parameters from the level above this one; 3 means gather parameters from
        /// the level above this one, followed by the level above that, and above that.
        /// </summary>
        [XmlAttribute]
        public int SearchParameterLevels
        {
            get { return _searchParamLevels; }
            set { _searchParamLevels = value; }
        }
        private int _searchParamLevels;

        /// <summary>
        /// Indicates if this node is a static node.  In other words, it doesn't gather results from the database, it is simply displayed verbatim.
        /// Static nodes ignore SearchParameterLevels but can specify CustomParameters to help narrow results below this node.
        /// </summary>
        [XmlIgnore]
        public bool IsStatic
        {
            get { return Subject == null; }
        }

        /// <summary>
        /// Additional parameters to use for children nodes to narrow their results.  Good for static nodes where you want to impose some parameters
        /// without a specific source.  For example, Static Node: "Launceston Store"; CustomParameters = (Location.Name = 'Launceston');
        /// </summary>
        [XmlElement]
        public IParameter CustomParameters
        {
            get { return _customParms; }
            set { _customParms = value; }
        }
        private IParameter _customParms;


        /// <summary>
        /// Child template nodes underneath this template node.
        /// </summary>
        [XmlArray]
        public List<TemplateNode> Children
        {
            get { return _children; }
            set { _children = value; }
        }
        private List<TemplateNode> _children;


        //TODO
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




        public TemplateNode()
        {
            // to perform xml serialisation, we need to link up the parent property and assign queryer/config
            _children = new List<TemplateNode>();
        }

        //TODO
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
