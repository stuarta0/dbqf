using dbqf.Criterion.Builders;
using dbqf.Serialization.DTO.Builders;
using dbqf.Sql.Criterion.Builders;

namespace dbqf.Serialization.Assemblers.Builders
{
    public class JunctionBuilderAssembler : BuilderAssembler
    {
        public JunctionBuilderAssembler(BuilderAssembler successor = null)
            : base(successor)
        {
        }

        public override IParameterBuilder Restore(ParameterBuilderDTO dto)
        {
            var sb = dto as JunctionBuilderDTO;
            if (sb == null)
                return base.Restore(dto);

            return new JunctionBuilder("Conjunction".Equals(sb.Type) ? JunctionType.Conjunction : JunctionType.Disjunction)
            {
                Label = sb.Label,
                Other = base.Chain.Restore(sb.Other)
            };
        }

        public override ParameterBuilderDTO Create(IParameterBuilder b)
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
