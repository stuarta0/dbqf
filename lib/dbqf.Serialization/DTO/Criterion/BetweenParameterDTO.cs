using System.Xml.Serialization;

namespace dbqf.Serialization.DTO.Criterion
{
    [XmlRoot("BetweenParameter")]
    public class BetweenParameterDTO : ParameterDTO
    {
        [XmlElement]
        public FieldPathDTO Path { get; set; }

        /// <summary>
        /// Gets or sets the high value of a BetweenParameter.
        /// </summary>
        [XmlElement]
        public object High { get; set; }
        public bool ShouldSerializeHigh() { return High != null; }

        /// <summary>
        /// Gets or sets the low value of a BetweenParameter.
        /// </summary>
        [XmlElement]
        public object Low { get; set; }
        public bool ShouldSerializeLow() { return Low != null; }
    }
}
