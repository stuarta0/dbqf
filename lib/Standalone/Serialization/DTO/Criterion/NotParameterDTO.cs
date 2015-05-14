using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Standalone.Serialization.DTO.Criterion
{
    [XmlRoot("NotParameter")]
    public class NotParameterDTO : ParameterDTO
    {
        /// <summary>
        /// Gets or sets a parameter that should be negated.
        /// </summary>
        [XmlText]
        public ParameterDTO Parameter { get; set; }

        public override void Accept(Assemblers.Criterion.IParameterDTOVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
