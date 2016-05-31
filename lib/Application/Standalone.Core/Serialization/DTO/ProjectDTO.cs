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

        // TODO: Ideally, this would be defined at the concrete class level rather than here. Not sure if that is possible with XmlSerializer.
        [XmlArrayItem(ElementName = "SqlConnection", Type = typeof(SqlProjectConnection))]
        [XmlArrayItem(ElementName = "SQLiteConnection", Type = typeof(SQLiteProjectConnection))]
        [XmlArrayItem(ElementName = "MsAccessConnection", Type = typeof(MsAccessProjectConnection))]
        public List<ProjectConnection> Connections { get; set; }

        public MatrixConfigurationDTO Configuration { get; set; }
    }
}
