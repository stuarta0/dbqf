using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Standalone.Serialization.DTO
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

        [ProtoMember(2)]
        public List<Standalone.Data.Connection> Connections { get; set; }

        [ProtoMember(3)]
        public ConfigurationDTO Configuration { get; set; }
    }
}
