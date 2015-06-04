using dbqf.Criterion;
using dbqf.Serialization.DTO.Criterion;

namespace dbqf.Serialization.Assemblers.Criterion
{
    public class NullParameterAssembler : ParameterAssembler
    {
        public NullParameterAssembler(FieldPathAssembler pathAssembler, AssemblyLine<IParameter, ParameterDTO> successor = null)
            : base(successor)
        {
            PathAssembler = pathAssembler;
        }

        public override dbqf.Criterion.IParameter Restore(DTO.Criterion.ParameterDTO dto)
        {
            var np = dto as NullParameterDTO;
            if (np == null)
                return base.Restore(dto);

            return new NullParameter(PathAssembler.Restore(np.Path));
        }

        public override DTO.Criterion.ParameterDTO Create(dbqf.Criterion.IParameter p)
        {
            var np = p as NullParameter;
            if (np == null)
                return base.Create(p);

            return new NullParameterDTO() { Path = PathAssembler.Create(np.Path) };
        }
    }
}
