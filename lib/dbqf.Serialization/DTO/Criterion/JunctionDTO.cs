using System.Collections.Generic;
using System.Xml.Serialization;

namespace dbqf.Serialization.DTO.Criterion
{
    [XmlRoot("Junction")]
    public class JunctionDTO : ParameterDTO
    {
        [XmlText]
        public IList<ParameterDTO> Parameters { get; set; }
    }
}
