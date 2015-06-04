using System.Xml.Serialization;
using dbqf.Serialization.DTO.Builders;
using dbqf.Serialization.DTO.Parsers;

namespace dbqf.Serialization.DTO.Display
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
