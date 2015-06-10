using dbqf.Criterion.Builders;
using dbqf.Serialization.DTO.Builders;

namespace dbqf.Serialization.Assemblers.Builders
{
    public class SimpleBuilderAssembler : BuilderAssembler
    {
        public SimpleBuilderAssembler(BuilderAssembler successor = null)
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
                Operator = sb.Operator
            };
        }
    }
}
