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
    public class BetweenBuilderAssembler : BuilderAssembler
    {
        public BetweenBuilderAssembler(BuilderAssembler successor = null)
            : base(successor)
        {
        }

        public override ParameterBuilder Restore(ParameterBuilderDTO dto)
        {
            var sb = dto as BetweenBuilderDTO;
            if (sb == null)
                return base.Restore(dto);

            return new BetweenBuilder()
            {
                Label = sb.Label
            };
        }

        public override ParameterBuilderDTO Create(ParameterBuilder b)
        {
            var sb = b as BetweenBuilder;
            if (sb == null)
                return base.Create(b);

            return new BetweenBuilderDTO() 
            { 
                Label = sb.Label
            };
        }
    }
}
