using dbqf.Criterion;
using Standalone.Serialization.DTO.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Serialization.Assemblers.Criterion
{
    public class NullParameterHandler : TransformHandler
    {
        private FieldPathAssembler _pathAssembler;
        public NullParameterHandler(TransformHandler successor, FieldPathAssembler pathAssembler)
            : base(successor)
        {
            _pathAssembler = pathAssembler;
        }

        public override dbqf.Criterion.IParameter Restore(DTO.Criterion.ParameterDTO dto)
        {
            var np = dto as NullParameterDTO;
            if (np == null)
                return base.Restore(dto);

            return new NullParameter(_pathAssembler.Restore(np.Path));
        }

        public override DTO.Criterion.ParameterDTO Create(dbqf.Criterion.IParameter p)
        {
            var np = p as NullParameter;
            if (np == null)
                return base.Create(p);

            return new NullParameterDTO() { Path = _pathAssembler.Create(np.Path) };
        }
    }
}
