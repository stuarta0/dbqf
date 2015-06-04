using System.Xml.Serialization;

namespace dbqf.Serialization.DTO.Builders
{
    [XmlRoot("LikeBuilder")]
    public class LikeBuilderDTO : ParameterBuilderDTO
    {
        /// <summary>
        /// Any of: Anywhere, Start, End, Exact
        /// </summary>
        [XmlAttribute]
        public string Mode { get; set; }
    }
}
