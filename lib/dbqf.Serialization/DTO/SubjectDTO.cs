
using System.Collections.Generic;
using System.Xml.Serialization;
namespace dbqf.Serialization.DTO
{
    [System.Diagnostics.DebuggerDisplay("{DisplayName} DTO")]
    public class SubjectDTO
    {
        [XmlElement]
        public string DisplayName { get; set; }
        
        [XmlElement]
        public string Source { get; set; }
        
        [XmlAttribute("IdField")]
        public int IdFieldIndex { get; set; }

        [XmlAttribute("DefaultField")]
        public int DefaultFieldIndex { get; set; }

        [XmlArrayItem("Field")]
        public List<FieldDTO> Fields { get; set; }

        public SubjectDTO()
        {
            Fields = new List<FieldDTO>();
        }
    }
}
