using Standalone.Serialization.DTO.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Serialization.Assemblers.Criterion
{
    public interface IParameterDTOVisitor
    {
        void Visit(SimpleParameterDTO dto);
        void Visit(LikeParameterDTO dto);
        void Visit(ConjunctionDTO dto);
        void Visit(NotParameterDTO dto);
        void Visit(NullParameterDTO dto);
        void Visit(BetweenParameterDTO dto);
    }
}
