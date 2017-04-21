using dbqf.Serialization.DTO.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Standalone.Core.Serialization.DTO.Parsers
{
    [XmlRoot("ExtendedDateParser")]
    public class ExtendedDateParserDTO : DateParserDTO
    {
        //[XmlAttribute]
        //public bool AllowNulls { get; set; }
    }
}
