using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using dbqf.Configuration;
using dbqf.Criterion;

namespace dbqf.Serialization.DTO
{
    [XmlRoot("FieldPath")]
    public class FieldPathDTO
    {
        [XmlAttribute("Subject")]
        public int SubjectIndex { get; set; }

        [XmlArray]
        public List<string> SourceNames { get; set; }

        public FieldPathDTO()
        {
            SourceNames = new List<string>();
        }
    }
}
