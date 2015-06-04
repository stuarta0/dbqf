using dbqf.Display.Builders;
using dbqf.Serialization.DTO.Builders;

namespace dbqf.Serialization.Assemblers.Builders
{
    public class NullBuilderAssembler : BuilderAssembler
    {
        public NullBuilderAssembler(BuilderAssembler successor = null)
            : base(successor)
        {
        }

        public override ParameterBuilder Restore(ParameterBuilderDTO dto)
        {
            var sb = dto as NullBuilderDTO;
            if (sb == null)
                return base.Restore(dto);

            return new NullBuilder()
            {
                Label = sb.Label
            };
        }

        public override ParameterBuilderDTO Create(ParameterBuilder b)
        {
            var sb = b as NullBuilder;
            if (sb == null)
                return base.Create(b);

            return new NullBuilderDTO() 
            { 
                Label = sb.Label
            };
        }
    }
}
