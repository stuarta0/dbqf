using System.Xml.Serialization;

namespace dbqf.Serialization.DTO.Builders
{
    [XmlRoot("BooleanBuilder")]
    public class BooleanBuilderDTO : ParameterBuilderDTO
    {
        [XmlAttribute]
        public bool Value { get; set; }
    }
}
