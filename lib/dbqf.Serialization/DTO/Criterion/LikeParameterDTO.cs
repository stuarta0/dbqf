using System.Xml.Serialization;

namespace dbqf.Serialization.DTO.Criterion
{
    [XmlRoot("LikeParameter")]
    public class LikeParameterDTO : ParameterDTO
    {
        [XmlElement]
        public FieldPathDTO Path { get; set; }
    }
}
