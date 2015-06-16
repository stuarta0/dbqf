using dbqf.Criterion.Builders;
using dbqf.Serialization.Assemblers.Criterion;
using dbqf.Serialization.DTO.Builders;

namespace dbqf.Serialization.Assemblers.Builders
{
    /// <summary>
    /// Handles conversion for ParameterBuilders that have no additional properties apart from base class property Label.
    /// This will work for Between, Null, and all DateXBuilders.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class BuilderAssembler<T, U> : BuilderAssembler
        where T : ParameterBuilder, new()
        where U : ParameterBuilderDTO, new()
    {
        public BuilderAssembler(BuilderAssembler successor = null)
            : base(successor)
        {
        }

        public override ParameterBuilder Restore(ParameterBuilderDTO dto)
        {
            var sb = dto as U;
            if (sb == null)
                return base.Restore(dto);

            return new T()
            {
                Label = sb.Label
            };
        }

        public override ParameterBuilderDTO Create(ParameterBuilder b)
        {
            var sb = b as T;
            if (sb == null)
                return base.Create(b);

            return new U()
            {
                Label = sb.Label
            };
        }
    }
}
