using dbqf.Criterion;
using Standalone.Serialization.DTO.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Serialization.Assemblers.Criterion
{
    public class ConjunctionParameterHandler : TransformHandler
    {
        private TransformHandler _chain;
        public ConjunctionParameterHandler(TransformHandler successor, TransformHandler chain)
            : base(successor)
        {
            // need a reference to the chain of responsibility that this TransformHandler is part of to restore the contained parameters
            _chain = chain;
        }

        public override dbqf.Criterion.IParameter Restore(DTO.Criterion.ParameterDTO dto)
        {
            var c = dto as ConjunctionDTO;
            if (c == null)
                return base.Restore(dto);

            var result = new Conjunction();
            foreach (var p in c.Parameters)
                result.Add(_chain.Restore(p));

            return result;
        }

        public override DTO.Criterion.ParameterDTO Create(dbqf.Criterion.IParameter p)
        {
            var c = p as Conjunction;
            if (c == null)
                return base.Create(p);

            var result = new ConjunctionDTO() { Parameters = new List<ParameterDTO>() };
            foreach (var p2 in c)
                result.Parameters.Add(_chain.Create(p2));

            return result;
        }
    }
}
