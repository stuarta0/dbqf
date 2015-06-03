using Standalone.Core.Serialization.DTO.Builders;
using Standalone.Core.Serialization.DTO.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Standalone.Core.Serialization.DTO.Display
{
    public class PartViewDTO
    {
        [XmlElement]
        public FieldPathDTO Path { get; set; }

        [XmlElement]
        public ParameterBuilderDTO Builder { get; set; }

        [XmlElement]
        public object[] Values { get; set; }

        [XmlElement]
        public ParserDTO Parser { get; set; }
    }
}
