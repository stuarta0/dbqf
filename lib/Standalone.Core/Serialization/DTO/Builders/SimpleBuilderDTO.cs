using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Standalone.Core.Serialization.DTO.Criterion;

namespace Standalone.Core.Serialization.DTO.Builders
{
    [XmlRoot("SimpleBuilder")]
    public class SimpleBuilderDTO : ParameterBuilderDTO
    {
        [XmlAttribute]
        public string Operator { get; set; }
    }
}
