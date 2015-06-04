using dbqf.Criterion;
using dbqf.Serialization.DTO.Criterion;

namespace dbqf.Serialization.Assemblers.Criterion
{
    public class NotParameterAssembler : ParameterAssembler
    {
        public NotParameterAssembler(AssemblyLine<IParameter, ParameterDTO> successor = null)
            : base(successor)
        {
        }

        public override dbqf.Criterion.IParameter Restore(DTO.Criterion.ParameterDTO dto)
        {
            var np = dto as NotParameterDTO;
            if (np == null)
                return base.Restore(dto);

            return new NotParameter(Chain.Restore(np.Parameter));
        }

        public override DTO.Criterion.ParameterDTO Create(dbqf.Criterion.IParameter p)
        {
            var np = p as NotParameter;
            if (np == null)
                return base.Create(p);

            return new NotParameterDTO() { Parameter = Chain.Create(np.Parameter) };
        }
    }
}
