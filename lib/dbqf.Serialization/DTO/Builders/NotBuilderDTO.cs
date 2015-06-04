using System.Xml.Serialization;

namespace dbqf.Serialization.DTO.Builders
{
    [XmlRoot("NotBuilder")]
    public class NotBuilderDTO : ParameterBuilderDTO
    {
        public ParameterBuilderDTO Other { get; set; }
    }
}
