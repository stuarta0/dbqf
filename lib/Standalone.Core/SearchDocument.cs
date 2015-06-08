using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Standalone.Core
{
    public class SearchDocument
    {
        /// <summary>
        /// Gets or sets details of the project file that was used to create this search document.
        /// </summary>
        public ProjectDetails Project { get; set; }

        /// <summary>
        /// Gets or sets a value indicating what search type was used
        /// to create this parameter (e.g. Preset, Standard, Advanced).
        /// </summary>
        public string SearchType { get; set; }

        /// <summary>
        /// Gets or sets the subject to use for fetching output.
        /// </summary>
        public ISubject Subject { get; set; }

        /// <summary>
        /// Gets or sets the set of parts that contain values for searching in this document.
        /// </summary>
        [XmlElement]
        public IList<IPartView> Parts { get; set; }

        /// <summary>
        /// Gets or sets the list of fields to use for custom output.  Null for default fields.
        /// </summary>
        [XmlArray]
        public List<FieldPath> Outputs { get; set; }

        public SearchDocument()
        { }
        public SearchDocument(Project p)
            : this()
        {
            Project = new ProjectDetails()
            {
                Id = p.Id,
                Version = p.Version
            };
        }
    }

    public class ProjectDetails
    {
        /// <summary>
        /// Gets or sets the project path that produced this parameter.
        /// Note: this is only a hint as to where the project file was located when saved. Not guaranteed to be correct on load.
        /// </summary>
        public string FileHint { get; set; }

        /// <summary>
        /// Gets or sets the project ID.  This will guarantee the project file is correct.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the version of the project file used to create this search.  Needs to be convertable to System.Version object.
        /// </summary>
        public Version Version { get; set; }
    }
}
