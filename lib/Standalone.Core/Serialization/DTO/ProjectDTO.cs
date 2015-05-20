using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Standalone.Core.Serialization.DTO
{
    [XmlRoot("Project")]
    [ProtoContract]
    public class ProjectDTO
    {
        /// <summary>
        /// Used as lookup to save the selected connection for a given project between sessions.
        /// </summary>
        [XmlAttribute]
        [ProtoMember(1)]
        public Guid Id { get; set; }

        [XmlElement]
        [ProtoMember(4)]
        public string Title { get; set; }
        public bool ShouldSerializeTitle() { return !String.IsNullOrWhiteSpace(Title); }

        [ProtoMember(2)]
        public List<Standalone.Core.Data.Connection> Connections { get; set; }

        [ProtoMember(3)]
        public ConfigurationDTO Configuration { get; set; }
    }
}
