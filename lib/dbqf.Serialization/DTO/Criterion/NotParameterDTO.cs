using System.Xml.Serialization;

namespace dbqf.Serialization.DTO.Criterion
{
    [XmlRoot("NotParameter")]
    public class NotParameterDTO : ParameterDTO
    {
        /// <summary>
        /// Gets or sets a parameter that should be negated.
        /// </summary>
        [XmlText]
        public ParameterDTO Parameter { get; set; }
    }
}
