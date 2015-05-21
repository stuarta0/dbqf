using dbqf.Criterion;
using Standalone.Core.Serialization.Assemblers.Criterion;
using Standalone.Core.Serialization.DTO.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Core.Serialization.Assemblers
{
    public class ParameterAssembler : IAssembler<IParameter, ParameterDTO>
    {
        private TransformHandler _chain;

        /// <summary>
        /// Given a chain of TransformHandlers, use this to restore or create the required DTOs.
        /// </summary>
        /// <param name="chain"></param>
        public ParameterAssembler(TransformHandler chain)
        {
            _chain = chain;
        }

        public IParameter Restore(ParameterDTO dto)
        {
            return _chain.Restore(dto);
        }

        public ParameterDTO Create(IParameter source)
        {
            return _chain.Create(source);
        }
    }
}
