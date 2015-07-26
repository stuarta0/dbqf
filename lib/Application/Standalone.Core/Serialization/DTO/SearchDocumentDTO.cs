using dbqf.Display;
using dbqf.Serialization.DTO;
using dbqf.Serialization.DTO.Criterion;
using dbqf.Serialization.DTO.Display;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Standalone.Core.Serialization.DTO
{
    public class SearchDocumentDTO
    {
        [XmlElement]
        public ProjectDetailsDTO Project { get; set; }

        [XmlAttribute("Type")]
        public string SearchType { get; set; }

        [XmlAttribute("Subject")]
        public int SubjectIndex { get; set; }

        [XmlElement]
        public PartViewDTO Parts { get; set; }

        [XmlArray]
        public List<FieldPathDTO> Outputs { get; set; }
    }

    public class ProjectDetailsDTO
    {
        /// <summary>
        /// Gets or sets the project path that produced this parameter.
        /// Note: this is only a hint as to where the project file was located when saved. Not guaranteed to be correct on load.
        /// </summary>
        [XmlElement]
        public string FileHint { get; set; }

        /// <summary>
        /// Gets or sets the project ID.  This will guarantee the project file is correct.
        /// </summary>
        [XmlElement]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the version of the project file used to create this search.  Needs to be convertable to System.Version object.
        /// </summary>
        [XmlElement]
        public string Version { get; set; }
    }
}
