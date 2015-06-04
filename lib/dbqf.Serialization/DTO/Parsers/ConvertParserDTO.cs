using System.Xml.Serialization;

namespace dbqf.Serialization.DTO.Parsers
{
    [XmlRoot("ConvertParser")]
    public class ConvertParserDTO : ParserDTO
    {
        [XmlAttribute]
        public string FromType { get; set; }

        [XmlAttribute]
        public string ToType { get; set; }
    }
}
