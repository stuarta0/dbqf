using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ProtoBuf;

namespace Standalone.Core.Serialization.DTO.Parsers
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
        public List<ParserDTO> Parsers { get; set; } 
    }
}
