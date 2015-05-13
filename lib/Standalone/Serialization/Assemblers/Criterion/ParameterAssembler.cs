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
        /*
         * Maybe we can use the visitor pattern on ParameterDTO to give type-safe Create/Restore here?
         * e.g.
         *  interface IParameterDTOVisitor
         *  {
         *      Visit(SimpleParameterDTO);
         *      Visit(LikeParameterDTO);
         *      Visit(ConjunctionDTO);
         *      etc...
         *  }
         *  interface IParameterDTO
         *  {
         *      Accept(IParameterDTOVisitor);
         *  }
         *  ParameterDTO.Accept(IParameterDTOVisitor pv)
         *  {
         *      pv.Visit(this);
         *  }
         *  
         *  ParameterDTO : IParameterDTO
         *  ParameterAssembler : IParameterDTOVisitor
         *  
         *  ParameterAssembler._restored;
         *  ParameterAssembler.Restore(ParameterDTO dto)
         *  {
         *      dto.Accept(this);
         *      return _restored;
         *  }
         *  ParameterAssembler.Visit(ConjunctionDTO dto)
         *  {
         *      var c = new Conjunction();
         *      foreach (var inner in dto.Parameters)
         *      {
         *          inner.Accept(this);
         *          c.Add(_restored);
         *      }
         *      _restored = c;
         *  }
         */

        public IParameter Restore(ParameterDTO dto)
        {
            throw new NotImplementedException();
        }

        public ParameterDTO Create(IParameter source)
        {
            throw new NotImplementedException();
        }
    }
}
