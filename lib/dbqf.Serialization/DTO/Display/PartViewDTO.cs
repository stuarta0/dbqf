using System.Xml.Serialization;
using dbqf.Serialization.DTO.Builders;
using dbqf.Serialization.DTO.Parsers;
using System.Collections.Generic;

namespace dbqf.Serialization.DTO.Display
{
    public class PartViewDTO
    {
        #region IPartViewNode Properties

        [XmlElement]
        public FieldPathDTO Path { get; set; }

        [XmlElement]
        public ParameterBuilderDTO Builder { get; set; }

        [XmlElement]
        public object[] Values { get; set; }

        [XmlElement]
        public ParserDTO Parser { get; set; }

        #endregion

        #region IPartViewJunction Properties

        [XmlAttribute]
        public string JunctionType { get; set; }

        [XmlElement("PartView")]
        public List<PartViewDTO> JunctionPartViews { get; set; }

        #endregion
    }
}
