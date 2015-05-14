using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Serialization.Assemblers.Criterion
{
    public class RestoreVisitor : IParameterDTOVisitor
    {
        private FieldPathAssembler _pathAssembler;
        public RestoreVisitor(FieldPathAssembler pathAssembler)
        {
            _pathAssembler = pathAssembler;
        }

        /// <summary>
        /// Gets the Parameter constructed after visiting the equivalent ParameterDTO.
        /// </summary>
        public IParameter Parameter { get; private set; }

        public void Visit(DTO.Criterion.SimpleParameterDTO dto)
        {
            Parameter = new SimpleParameter(_pathAssembler.Restore(dto.Path), dto.Operator, dto.Value);
        }

        public void Visit(DTO.Criterion.LikeParameterDTO dto)
        {
            // TODO
            Parameter = new LikeParameter(_pathAssembler.Restore(dto.Path), null);
        }

        public void Visit(DTO.Criterion.ConjunctionDTO dto)
        {
            var restorer = new RestoreVisitor(_pathAssembler);
            var con = new Conjunction();
            foreach (var dto2 in dto.Parameters)
            {
                dto2.Accept(restorer);
                con.Add(restorer.Parameter);
            }
            Parameter = con;
        }

        public void Visit(DTO.Criterion.NotParameterDTO dto)
        {
            var restorer = new RestoreVisitor(_pathAssembler);
            dto.Parameter.Accept(restorer);
            Parameter = new NotParameter(restorer.Parameter);
        }

        public void Visit(DTO.Criterion.NullParameterDTO dto)
        {
            Parameter = new NullParameter(_pathAssembler.Restore(dto.Path));
        }

        public void Visit(DTO.Criterion.BetweenParameterDTO dto)
        {
            Parameter = new BetweenParameter(_pathAssembler.Restore(dto.Path), dto.Low, dto.High);
        }
    }
}
