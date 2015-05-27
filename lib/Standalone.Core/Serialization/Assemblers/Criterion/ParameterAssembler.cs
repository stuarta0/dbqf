using dbqf.Criterion;
using Standalone.Core.Serialization.Assemblers.Criterion;
using Standalone.Core.Serialization.DTO.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Core.Serialization.Assemblers.Criterion
{
    public class ParameterAssembler : AssemblyLine<IParameter, ParameterDTO>
    {
        // need a reference to the root of the chain of responsibility to restore the contained parameters
        public AssemblyLine<IParameter, ParameterDTO> Chain { get; set; }
        public FieldPathAssembler PathAssembler { get; set; }
        public ParameterAssembler(AssemblyLine<IParameter, ParameterDTO> successor)
            : base(successor)
        {
        }
    }
}
