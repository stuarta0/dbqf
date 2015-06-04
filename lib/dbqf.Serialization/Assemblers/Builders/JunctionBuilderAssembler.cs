using dbqf.Display.Builders;
using dbqf.Serialization.DTO.Builders;

namespace dbqf.Serialization.Assemblers.Builders
{
    public class JunctionBuilderAssembler : BuilderAssembler
    {
        public JunctionBuilderAssembler(BuilderAssembler successor = null)
            : base(successor)
        {
        }

        public override ParameterBuilder Restore(ParameterBuilderDTO dto)
        {
            var sb = dto as JunctionBuilderDTO;
            if (sb == null)
                return base.Restore(dto);

            return new JunctionBuilder("Conjunction".Equals(sb.Type) ? JunctionBuilder.JunctionType.Conjunction : JunctionBuilder.JunctionType.Disjunction)
            {
                Label = sb.Label,
                Other = base.Chain.Restore(sb.Other)
            };
        }

        public override ParameterBuilderDTO Create(ParameterBuilder b)
        {
            var sb = b as JunctionBuilder;
            if (sb == null)
                return base.Create(b);

            return new JunctionBuilderDTO() 
            { 
                Label = sb.Label,
                Type = sb.Type.ToString(),
                Other = base.Chain.Create(sb.Other)
            };
        }
    }
}
