using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DbQueryFramework_v1.Configuration
{
    /// <summary>
    /// Represents a node in the query matrix of how to get from one subject to another.
    /// </summary>
    public class MatrixNode
    {
        /// <summary>
        /// Gets or sets what tool tip should be displayed between this node from/to.  For example, from issue to case: "Search for cases that have an issue" which will then be concatenated with search value "where product name = X" which gives "Search for cases that have an issue where product name = X".
        /// </summary>
        [XmlElement]
        public string ToolTip { get; set; }

        /// <summary>
        /// Gets or sets the query that represents the relationship from the source to the target.  Must contain columns FromID and ToID; does not need to be unique; shouldn't contain null IDs.
        /// </summary>
        [XmlElement]
        public string Query { get; set; }


        public static MatrixNode Empty
        {
            get { return new MatrixNode(); }
        }

        public MatrixNode()
        {

        }
    }
}
