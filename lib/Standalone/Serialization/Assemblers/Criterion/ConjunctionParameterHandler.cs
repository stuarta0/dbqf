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
        // need a reference to the chain of responsibility that this TransformHandler is part of to restore the contained parameters
        public TransformHandler Chain { get; set; }
        public ConjunctionParameterHandler(TransformHandler successor)
            : base(successor)
        {
        }

        public override dbqf.Criterion.IParameter Restore(DTO.Criterion.ParameterDTO dto)
        {
            var c = dto as ConjunctionDTO;
            if (c == null)
                return base.Restore(dto);

            var result = new Conjunction();
            foreach (var p in c.Parameters)
                result.Add(Chain.Restore(p));

            return result;
        }

        public override DTO.Criterion.ParameterDTO Create(dbqf.Criterion.IParameter p)
        {
            var c = p as Conjunction;
            if (c == null)
                return base.Create(p);

            var result = new ConjunctionDTO() { Parameters = new List<ParameterDTO>() };
            foreach (var p2 in c)
                result.Parameters.Add(Chain.Create(p2));

            return result;
        }
    }
}
