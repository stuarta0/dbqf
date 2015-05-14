using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Standalone.Serialization.DTO.Criterion
{
    [XmlRoot("NullParameter")]
    public class NullParameterDTO : ParameterDTO
    {
        [XmlElement]
        public FieldPathDTO Path { get; set; }

        public override void Accept(Assemblers.Criterion.IParameterDTOVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
