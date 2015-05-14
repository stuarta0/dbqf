using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Serialization.Assemblers.Criterion
{
    public interface IParameterDTO
    {
        void Accept(IParameterDTOVisitor visitor);
    }
}
