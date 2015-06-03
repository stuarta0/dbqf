using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ProtoBuf;

namespace Standalone.Core.Serialization.DTO.Parsers
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
