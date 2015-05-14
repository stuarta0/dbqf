using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Criterion;
using ProtoBuf;
using System.Xml.Serialization;
using System.ComponentModel;
using Standalone.Serialization.Assemblers.Criterion;

namespace Standalone.Serialization.DTO.Criterion
{
    [XmlRoot("Parameter")]
    public abstract class ParameterDTO : IParameterDTO
    {
        public abstract void Accept(IParameterDTOVisitor visitor);
    }
}
