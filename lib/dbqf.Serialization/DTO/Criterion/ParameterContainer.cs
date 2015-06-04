using System.Collections.Generic;
using System.Xml.Serialization;

namespace dbqf.Serialization.DTO.Criterion
{
    public class ParameterContainer
    {
        /// <summary>
        /// Gets or sets the project path that produced this parameter.  
        /// TODO: change to better identification mechanism.
        /// </summary>
        [XmlElement]
        public string ProjectFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating what search type was used
        /// to create this parameter (e.g. Preset, Standard, Advanced).
        /// </summary>
        [XmlAttribute("Type")]
        public string SearchType { get; set; }

        /// <summary>
        /// Gets or sets the subject index to use for fetching output.
        /// </summary>
        [XmlAttribute("Subject")]
        public int SubjectIndex { get; set; }

        /// <summary>
        /// Gets or sets the parameter to limit the result set.
        /// </summary>
        [XmlElement]
        public ParameterDTO Parameter { get; set; }

        /// <summary>
        /// Gets or sets the list of fields to use for custom output.
        /// </summary>
        [XmlArray]
        public List<FieldPathDTO> Outputs { get; set; }
    }
}
