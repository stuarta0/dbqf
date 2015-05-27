using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Criterion;
using ProtoBuf;
using System.Xml.Serialization;

namespace Standalone.Core.Serialization.DTO.Criterion
{
    [XmlRoot("Junction")]
    public class JunctionDTO : ParameterDTO
    {
        [XmlText]
        public IList<ParameterDTO> Parameters { get; set; }
    }
}
