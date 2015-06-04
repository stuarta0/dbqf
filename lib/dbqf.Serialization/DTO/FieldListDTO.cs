using System.Collections.Generic;
using System.Xml.Serialization;
using dbqf.Configuration;

namespace dbqf.Serialization.DTO
{
    public class FieldListDTO
    {
        public string SourceSql { get; set; }

        [XmlAttribute]
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
