using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Standalone.Core.Serialization.DTO.Criterion;

namespace Standalone.Core.Serialization.DTO.Builders
{
    [XmlInclude(typeof(JunctionBuilderDTO))]
    [XmlInclude(typeof(LikeBuilderDTO))]
    public abstract class ParameterBuilderDTO
    {
        [XmlAttribute]
        public string Label { get; set; }
    }
}
