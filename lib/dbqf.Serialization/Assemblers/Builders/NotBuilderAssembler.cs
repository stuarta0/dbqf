using dbqf.Criterion.Builders;
using dbqf.Serialization.DTO.Builders;
using dbqf.Sql.Criterion.Builders;

namespace dbqf.Serialization.Assemblers.Builders
{
    public class NotBuilderAssembler : BuilderAssembler
    {
        public NotBuilderAssembler(BuilderAssembler successor = null)
            : base(successor)
        {
        }

        public override IParameterBuilder Restore(ParameterBuilderDTO dto)
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

        public override ParameterBuilderDTO Create(IParameterBuilder b)
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
