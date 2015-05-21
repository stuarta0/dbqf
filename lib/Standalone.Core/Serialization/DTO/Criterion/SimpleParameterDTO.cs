using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Standalone.Core.Serialization.DTO.Criterion
{
    [XmlRoot("SimpleParameter")]
    public class SimpleParameterDTO : ParameterDTO
    {
        [XmlElement]
        public FieldPathDTO Path { get; set; }

        /// <summary>
        /// Gets or sets the operator of a SimpleParameter.
        /// </summary>
        [XmlAttribute]
        public string Operator { get; set; }
        public bool ShouldSerializeOperator() { return !String.IsNullOrWhiteSpace(Operator); }

        /// <summary>
        /// Gets or sets the value of a SimpleParameter.
        /// </summary>
        [XmlElement]
        public object Value { get; set; }
        public bool ShouldSerializeValue() { return Value != null; }
    }
}
