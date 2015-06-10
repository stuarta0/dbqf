using System.Xml.Serialization;

namespace dbqf.Serialization.DTO.Parsers
{
    [XmlRoot("DateParser")]
    public class DateParserDTO : ParserDTO
    {
        [XmlAttribute]
        public bool AllowNulls { get; set; }
    }
}
