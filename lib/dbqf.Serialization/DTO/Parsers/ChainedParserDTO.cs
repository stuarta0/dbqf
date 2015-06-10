using System.Collections.Generic;
using System.Xml.Serialization;

namespace dbqf.Serialization.DTO.Parsers
{
    [XmlRoot("ChainedParser")]
    public class ChainedParserDTO : ParserDTO
    {
        public ChainedParserDTO()
        {
            Parsers = new List<ParserDTO>();
        }

        [XmlArray]
        [XmlArrayItem("DelimitedParser", typeof(DelimitedParserDTO))]
        [XmlArrayItem("ChainedParser", typeof(ChainedParserDTO))]
        [XmlArrayItem("ConvertParser", typeof(ConvertParserDTO))]
        [XmlArrayItem("DateParser", typeof(DateParserDTO))]
        public List<ParserDTO> Parsers { get; set; } 
    }
}
