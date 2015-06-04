using System.Xml.Serialization;

namespace dbqf.Serialization.DTO.Builders
{
    [XmlRoot("JunctionBuilder")]
    public class JunctionBuilderDTO : ParameterBuilderDTO
    {
        [XmlAttribute]
        public string Type { get; set; }

        [XmlElement]
        public ParameterBuilderDTO Other { get; set; }
    }
}
