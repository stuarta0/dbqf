using System.Xml.Serialization;

namespace dbqf.Serialization.DTO.Builders
{
    public abstract class ParameterBuilderDTO
    {
        [XmlAttribute]
        public string Label { get; set; }
    }
}
