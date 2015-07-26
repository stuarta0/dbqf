using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using dbqf.Serialization.DTO;

namespace Standalone.Core.Serialization.DTO
{
    [XmlRoot("Project")]
    public class ProjectDTO
    {
        /// <summary>
        /// Used as lookup to save the selected connection for a given project between sessions.
        /// </summary>
        [XmlAttribute]
        public Guid Id { get; set; }

        [XmlElement]
        public string Title { get; set; }
        public bool ShouldSerializeTitle() { return !String.IsNullOrWhiteSpace(Title); }

        public List<Connection> Connections { get; set; }

        public ConfigurationDTO Configuration { get; set; }
    }
}
