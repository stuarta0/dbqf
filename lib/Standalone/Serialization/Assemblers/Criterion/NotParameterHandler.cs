using dbqf.Criterion;
using Standalone.Serialization.DTO.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Serialization.Assemblers.Criterion
{
    public class NotParameterHandler : TransformHandler
    {
        private TransformHandler _chain;
        public NotParameterHandler(TransformHandler successor, TransformHandler chain)
            : base(successor)
        {
            // need a reference to the chain of responsibility that this TransformHandler is part of to restore the contained parameter
            _chain = chain;
        }

        public override dbqf.Criterion.IParameter Restore(DTO.Criterion.ParameterDTO dto)
        {
            var np = dto as NotParameterDTO;
            if (np == null)
                return base.Restore(dto);

            return new NotParameter(_chain.Restore(np.Parameter));
        }

        public override DTO.Criterion.ParameterDTO Create(dbqf.Criterion.IParameter p)
        {
            var np = p as NotParameter;
            if (np == null)
                return base.Create(p);

            return new NotParameterDTO() { Parameter = _chain.Create(np.Parameter) };
        }
    }
}
