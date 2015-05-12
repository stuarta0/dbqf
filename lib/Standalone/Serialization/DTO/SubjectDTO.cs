using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Standalone.Serialization.DTO
{
    [ProtoContract]
    public class SubjectDTO
    {
        [ProtoMember(1)]
        public string DisplayName { get; set; }
        
        [ProtoMember(2)]
        public string Source { get; set; }
        
        [XmlAttribute("IdField")]
        [ProtoMember(3)]
        public int IdFieldIndex { get; set; }

        [XmlAttribute("DefaultField")]
        [ProtoMember(4)]
        public int DefaultFieldIndex { get; set; }

        [XmlArrayItem("Field")]
        [ProtoMember(5)]
        public List<FieldDTO> Fields { get; set; }

        public SubjectDTO()
        {
            Fields = new List<FieldDTO>();
        }
    }
}
