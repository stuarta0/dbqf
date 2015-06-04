using dbqf.Criterion;
using dbqf.Serialization.DTO.Criterion;

namespace dbqf.Serialization.Assemblers.Criterion
{
    public class SimpleParameterAssembler : ParameterAssembler
    {
        public SimpleParameterAssembler(FieldPathAssembler pathAssembler, AssemblyLine<IParameter, ParameterDTO> successor = null)
            : base(successor)
        {
            PathAssembler = pathAssembler;
        }

        public override dbqf.Criterion.IParameter Restore(DTO.Criterion.ParameterDTO dto)
        {
            var sp = dto as SimpleParameterDTO;
            if (sp == null)
                return base.Restore(dto);

            return new SimpleParameter(PathAssembler.Restore(sp.Path), sp.Operator, sp.Value);
        }

        public override DTO.Criterion.ParameterDTO Create(dbqf.Criterion.IParameter p)
        {
            var sp = p as SimpleParameter;
            if (sp == null)
                return base.Create(p);

            return new SimpleParameterDTO() 
            { 
                Path = PathAssembler.Create(sp.Path), 
                Operator = sp.Operator, 
                Value = sp.Value  
            };
        }
    }
}
