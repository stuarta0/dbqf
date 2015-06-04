using dbqf.Display.Builders;
using dbqf.Serialization.Assemblers.Criterion;
using dbqf.Serialization.DTO.Builders;

namespace dbqf.Serialization.Assemblers.Builders
{
    public abstract class BuilderAssembler : AssemblyLine<ParameterBuilder, ParameterBuilderDTO>
    {
        // need a reference to the root of the chain of responsibility to restore any nested builders
        public BuilderAssembler Chain { get; set; }
        public BuilderAssembler(BuilderAssembler successor = null)
            : base(successor)
        {
        }
    }
}
