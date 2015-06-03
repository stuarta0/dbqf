using dbqf.Criterion;
using Standalone.Core.Serialization.DTO.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Core.Serialization.Assemblers.Criterion
{
    public abstract class AssemblyLine<TSource, TDto> : IAssembler<TSource, TDto>
    {
        public AssemblyLine<TSource, TDto> Successor { get { return _successor; } }
        private AssemblyLine<TSource, TDto> _successor;
        protected AssemblyLine(AssemblyLine<TSource, TDto> successor = null)
        {
            _successor = successor;
        }

        /// <summary>
        /// Fluently add the successor and return a reference to the successor.
        /// </summary>
        /// <param name="successor"></param>
        /// <returns></returns>
        public AssemblyLine<TSource, TDto> Add(AssemblyLine<TSource, TDto> successor)
        {
            _successor = successor;
            return _successor;
        }

        public virtual TSource Restore(TDto dto)
        {
            if (_successor != null)
                return _successor.Restore(dto);
            return default(TSource);
        }

        public virtual TDto Create(TSource p)
        {
            if (_successor != null)
                return _successor.Create(p);
            return default(TDto);
        }
    }
}
