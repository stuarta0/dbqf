using System;
using System.Xml.Serialization;

namespace dbqf.Serialization.DTO.Criterion
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
        public bool ShouldSerializeOperator() { return !String.IsNullOrEmpty(Operator); }

        /// <summary>
        /// Gets or sets the value of a SimpleParameter.
        /// </summary>
        [XmlElement]
        public object Value { get; set; }
        public bool ShouldSerializeValue() { return Value != null; }
    }
}
