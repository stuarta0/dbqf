using dbqf.Criterion;
using dbqf.Serialization.DTO.Criterion;

namespace dbqf.Serialization.Assemblers.Criterion
{
    public class ParameterAssembler : AssemblyLine<IParameter, ParameterDTO>
    {
        // need a reference to the root of the chain of responsibility to restore the contained parameters
        public AssemblyLine<IParameter, ParameterDTO> Chain { get; set; }
        public FieldPathAssembler PathAssembler { get; set; }
        public ParameterAssembler(AssemblyLine<IParameter, ParameterDTO> successor = null)
            : base(successor)
        {
        }
    }
}
