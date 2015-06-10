using dbqf.Criterion.Builders;
using dbqf.Serialization.DTO.Builders;

namespace dbqf.Serialization.Assemblers.Builders
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
