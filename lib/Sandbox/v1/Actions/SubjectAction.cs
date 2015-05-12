using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DbQueryFramework_v1.Actions
{
    /// <summary>
    /// Represents an action to perform when a Subject is chosen in a user interface.
    /// </summary>
    [Serializable]
    public abstract class SubjectAction
    {
        /// <summary>
        /// Gets or sets the description of what this action will perform.
        /// </summary>
        [XmlElement]
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets whether this action is only valid if all placeholders are a non-null value.
        /// </summary>
        [XmlAttribute]
        public virtual bool RequirePlaceholders { get; set; }

        /// <summary>
        /// Gets or sets the image key to index an image when using dbqf.IO.QuerySetup.Images.
        /// </summary>
        [XmlElement]
        public virtual string ImageKey { get; set; }

    }
}
