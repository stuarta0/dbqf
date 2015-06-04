using System.Xml.Serialization;

namespace dbqf.Serialization.DTO.Builders
{
    [XmlRoot("SimpleBuilder")]
    public class SimpleBuilderDTO : ParameterBuilderDTO
    {
        [XmlAttribute]
        public string Operator { get; set; }
    }
}
