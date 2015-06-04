using System.Xml.Serialization;

namespace dbqf.Serialization.DTO.Criterion
{
    [XmlRoot("NullParameter")]
    public class NullParameterDTO : ParameterDTO
    {
        [XmlElement]
        public FieldPathDTO Path { get; set; }
    }
}
