using System.Xml.Serialization;
using dbqf.Display;

namespace dbqf.Serialization.DTO.Criterion
{
    public class BetweenValueDTO
    {
        [XmlElement]
        public object From { get; set; }
        public bool ShouldSerializeFrom() { return From != null; }

        [XmlElement]
        public object To { get; set; }
        public bool ShouldSerializeTo() { return To != null; }

    }
}
