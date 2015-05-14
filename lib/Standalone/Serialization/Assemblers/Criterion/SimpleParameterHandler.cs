using dbqf.Criterion;
using Standalone.Serialization.DTO.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Serialization.Assemblers.Criterion
{
    public class SimpleParameterHandler : TransformHandler
    {
        private FieldPathAssembler _pathAssembler;
        public SimpleParameterHandler(TransformHandler successor, FieldPathAssembler pathAssembler)
            : base(successor)
        {
            _pathAssembler = pathAssembler;
        }

        public override dbqf.Criterion.IParameter Restore(DTO.Criterion.ParameterDTO dto)
        {
            var sp = dto as SimpleParameterDTO;
            if (sp == null)
                return base.Restore(dto);

            return new SimpleParameter(_pathAssembler.Restore(sp.Path), sp.Operator, sp.Value);
        }

        public override DTO.Criterion.ParameterDTO Create(dbqf.Criterion.IParameter p)
        {
            var sp = p as SimpleParameter;
            if (sp == null)
                return base.Create(p);

            return new SimpleParameterDTO() 
            { 
                Path = _pathAssembler.Create(sp.Path), 
                Operator = sp.Operator, 
                Value = sp.Value  
            };
        }
    }
}
