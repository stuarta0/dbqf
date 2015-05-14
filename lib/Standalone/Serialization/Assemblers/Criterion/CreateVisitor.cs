using dbqf.Criterion;
using Standalone.Serialization.DTO.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Serialization.Assemblers.Criterion
{
    public class CreateVisitor : IParameterDTOVisitor
    {
        /// <summary>
        /// Gets the ParameterDTO constructed after visiting the equivalent IParameter.
        /// </summary>
        public ParameterDTO Parameter { get; private set; }

        public void Visit(DTO.Criterion.SimpleParameterDTO dto)
        {
            throw new NotImplementedException();
        }

        public void Visit(DTO.Criterion.LikeParameterDTO dto)
        {
            throw new NotImplementedException();
        }

        public void Visit(DTO.Criterion.ConjunctionDTO dto)
        {
            throw new NotImplementedException();
        }

        public void Visit(DTO.Criterion.NotParameterDTO dto)
        {
            throw new NotImplementedException();
        }

        public void Visit(DTO.Criterion.NullParameterDTO dto)
        {
            throw new NotImplementedException();
        }

        public void Visit(DTO.Criterion.BetweenParameterDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
