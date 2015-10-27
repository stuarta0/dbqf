using dbqf.Criterion.Builders;
using dbqf.Serialization.DTO.Builders;
using dbqf.Sql.Criterion.Builders;

namespace dbqf.Serialization.Assemblers.Builders
{
    public class SimpleBuilderAssembler : BuilderAssembler
    {
        public SimpleBuilderAssembler(BuilderAssembler successor = null)
            : base(successor)
        {
        }

        public override IParameterBuilder Restore(ParameterBuilderDTO dto)
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

        public override ParameterBuilderDTO Create(IParameterBuilder b)
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
