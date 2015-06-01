using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Display.Builders;
using Standalone.Core.Serialization.DTO.Builders;
using Standalone.Core.Serialization.Assemblers.Criterion;

namespace Standalone.Core.Serialization.Assemblers.Builders
{
    public abstract class BuilderAssembler : AssemblyLine<ParameterBuilder, ParameterBuilderDTO>
    {
        // need a reference to the root of the chain of responsibility to restore any nested builders
        public AssemblyLine<ParameterBuilder, ParameterBuilderDTO> Chain { get; set; }
        public BuilderAssembler(AssemblyLine<ParameterBuilder, ParameterBuilderDTO> successor)
            : base(successor)
        {
        }
    }
}
