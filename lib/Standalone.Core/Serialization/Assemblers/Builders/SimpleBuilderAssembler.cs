using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Display.Builders;
using Standalone.Core.Serialization.DTO.Builders;
using Standalone.Core.Serialization.DTO.Criterion;

namespace Standalone.Core.Serialization.Assemblers.Builders
{
    public class SimpleBuilderAssembler : BuilderAssembler
    {
        public SimpleBuilderAssembler(BuilderAssembler successor)
            : base(successor)
        {
        }

        public override ParameterBuilder Restore(ParameterBuilderDTO dto)
        {
            var sb = dto as SimpleBuilderDTO;
            if (sb == null)
                return base.Restore(dto);

            return new SimpleBuilder()
            {
                Label = sb.Label,
                Junction = (Junction)JunctionAssembler.Restore(sb.Junction),
                Operator = sb.Operator
            };
        }

        public override ParameterBuilderDTO Create(ParameterBuilder b)
        {
            var sb = b as SimpleBuilder;
            if (sb == null)
                return base.Create(b);

            return new SimpleBuilderDTO() 
            { 
                Label = sb.Label,
                Junction = (JunctionDTO)JunctionAssembler.Create(sb.Junction),
                Operator = sb.Operator
            };
        }
    }
}
