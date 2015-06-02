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
    public class NotBuilderAssembler : BuilderAssembler
    {
        public NotBuilderAssembler(BuilderAssembler successor = null)
            : base(successor)
        {
        }

        public override ParameterBuilder Restore(ParameterBuilderDTO dto)
        {
            var sb = dto as NotBuilderDTO;
            if (sb == null)
                return base.Restore(dto);

            return new NotBuilder()
            {
                Label = sb.Label,
                Other = Chain.Restore(sb.Other)
            };
        }

        public override ParameterBuilderDTO Create(ParameterBuilder b)
        {
            var sb = b as NotBuilder;
            if (sb == null)
                return base.Create(b);

            return new NotBuilderDTO() 
            { 
                Label = sb.Label,
                Other = Chain.Create(sb.Other)
            };
        }
    }
}
