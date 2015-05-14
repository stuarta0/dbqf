using dbqf.Criterion;
using Standalone.Serialization.DTO.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Serialization.Assemblers.Criterion
{
    public class ParameterAssembler : IAssembler<IParameter, ParameterDTO>
    {
        private FieldPathAssembler _pathAssembler;
        public ParameterAssembler(FieldPathAssembler pathAssembler)
        {
            _pathAssembler = pathAssembler;
        }

        public IParameter Restore(ParameterDTO dto)
        {
            var restorer = new RestoreVisitor(_pathAssembler);
            dto.Accept(restorer);
            return restorer.Parameter;
        }

        public ParameterDTO Create(IParameter source)
        {
            throw new NotImplementedException();
        }
    }
}
