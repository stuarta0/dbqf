using dbqf.Criterion;
using Standalone.Core.Serialization.DTO.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Core.Serialization.Assemblers.Criterion
{
    public abstract class TransformHandler
    {
        private TransformHandler _successor;
        protected TransformHandler(TransformHandler successor)
        {
            _successor = successor;
        }

        public virtual IParameter Restore(ParameterDTO dto)
        {
            if (_successor != null)
                return _successor.Restore(dto);
            return null;
        }

        public virtual ParameterDTO Create(IParameter p)
        {
            if (_successor != null)
                return _successor.Create(p);
            return null;
        }
    }
}
