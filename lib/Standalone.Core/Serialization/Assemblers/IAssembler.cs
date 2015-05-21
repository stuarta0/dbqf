using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Core.Serialization.Assemblers
{
    public interface IAssembler<TSource, TDto>
    {
        /// <summary>
        /// Recreate a source object from it's DTO representation.
        /// </summary>
        TSource Restore(TDto dto);

        /// <summary>
        /// Create a DTO representation of a source object.
        /// </summary>
        TDto Create(TSource source);
    }
}
