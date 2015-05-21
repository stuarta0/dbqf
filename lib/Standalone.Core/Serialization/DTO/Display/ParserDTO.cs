using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ProtoBuf;

namespace Standalone.Core.Serialization.DTO.Display
{
    [ProtoContract]
    public class ParserDTO
    {
        /// <summary>
        /// Gets or sets the parser type name.
        /// </summary>
        [XmlAttribute]
        [ProtoMember(1)]
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets delimiters to use with DelimitedParser.
        /// </summary>
        [ProtoMember(2)]
        public string[] Delimiters { get; set; } 
    }
}
