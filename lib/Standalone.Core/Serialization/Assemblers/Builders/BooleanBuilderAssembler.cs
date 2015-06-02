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
    public class BooleanBuilderAssembler : BuilderAssembler
    {
        public BooleanBuilderAssembler(BuilderAssembler successor = null)
            : base(successor)
        {
        }

        public override ParameterBuilder Restore(ParameterBuilderDTO dto)
        {
            var sb = dto as BooleanBuilderDTO;
            if (sb == null)
                return base.Restore(dto);

            return new BooleanBuilder()
            {
                Label = sb.Label,
                Value = sb.Value
            };
        }

        public override ParameterBuilderDTO Create(ParameterBuilder b)
        {
            var sb = b as BooleanBuilder;
            if (sb == null)
                return base.Create(b);

            return new BooleanBuilderDTO() 
            { 
                Label = sb.Label,
                Value = sb.Value
            };
        }
    }
}
