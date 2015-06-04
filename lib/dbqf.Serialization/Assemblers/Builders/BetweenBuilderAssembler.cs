using dbqf.Display.Builders;
using dbqf.Serialization.DTO.Builders;

namespace dbqf.Serialization.Assemblers.Builders
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
