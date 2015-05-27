using dbqf.Criterion;
using Standalone.Core.Serialization.DTO.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Core.Serialization.Assemblers.Criterion
{
    public class SimpleParameterAssembler : ParameterAssembler
    {
        public SimpleParameterAssembler(AssemblyLine<IParameter, ParameterDTO> successor, FieldPathAssembler pathAssembler)
            : base(successor)
        {
            PathAssembler = pathAssembler;
        }

        public override dbqf.Criterion.IParameter Restore(DTO.Criterion.ParameterDTO dto)
        {
            var sp = dto as SimpleParameterDTO;
            if (sp == null)
                return base.Restore(dto);

            return new SimpleParameter(PathAssembler.Restore(sp.Path), sp.Operator, sp.Value);
        }

        public override DTO.Criterion.ParameterDTO Create(dbqf.Criterion.IParameter p)
        {
            var sp = p as SimpleParameter;
            if (sp == null)
                return base.Create(p);

            return new SimpleParameterDTO() 
            { 
                Path = PathAssembler.Create(sp.Path), 
                Operator = sp.Operator, 
                Value = sp.Value  
            };
        }
    }
}
