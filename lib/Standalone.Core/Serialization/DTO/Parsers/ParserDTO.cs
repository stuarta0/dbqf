using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ProtoBuf;

namespace Standalone.Core.Serialization.DTO.Parsers
{
    [XmlRoot("Parser")]
    [XmlInclude(typeof(DelimitedParserDTO))]
    public abstract class ParserDTO
    {
    }
}
