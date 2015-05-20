using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using dbqf.Configuration;
using ProtoBuf;

namespace Standalone.Core.Serialization.DTO
{
    public class FieldListDTO
    {
        [ProtoMember(1)]
        public string SourceSql { get; set; }

        [XmlAttribute]
        [ProtoMember(2)]
        public FieldListType Type { get; set; }

        // Not suitable for ProtoBuf
        public List<object> Items { get; set; }

        /// <summary>
        /// Controls XML serialisation to emit Items only when there is a non-empty list instance.
        /// </summary>
        [XmlIgnore]
        public bool ItemsSpecified
        {
            get { return Items != null && Items.Count > 0; }
        }
    }
}
