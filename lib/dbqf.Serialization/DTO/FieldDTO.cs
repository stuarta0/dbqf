using System.Collections.Generic;
using System.Xml.Serialization;
using dbqf.Serialization.DTO.Parsers;

namespace dbqf.Serialization.DTO
{
    public class FieldDTO
    {
        #region IField

        //ISubject Parent - can be set by assembler

        [XmlAttribute("Source")]
        public string SourceName { get; set; }

        [XmlElement("Type")]
        public string DataTypeFullName { get; set; }

        [XmlElement]
        public string DisplayName { get; set; }

        [XmlElement]
        public string DisplayFormat { get; set; }

        [XmlElement]
        public FieldListDTO List { get; set; }

        #endregion

        #region IRelationField

        [System.ComponentModel.DefaultValue(-1)]
        public int RelatedSubjectIndex { get; set; }

        [XmlElement("OutputType")]
        public string OutputDataTypeFullName { get; set; }

        [XmlElement("OutputSource")]
        public string OutputSourceName { get; set; }

        #endregion

        #region dbqf Properties

        [XmlArray]
        [XmlArrayItem("DelimitedParser", typeof(DelimitedParserDTO))]
        [XmlArrayItem("ChainedParser", typeof(ChainedParserDTO))]
        [XmlArrayItem("ConvertParser", typeof(ConvertParserDTO))]
        public List<ParserDTO> Parsers { get; set; }

        #endregion

        public FieldDTO()
        {
            RelatedSubjectIndex = -1;
            Parsers = new List<ParserDTO>();
        }
    }
}
