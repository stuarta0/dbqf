using System.Collections.Generic;
using dbqf.Criterion;
using dbqf.Serialization.DTO.Criterion;

namespace dbqf.Serialization.Assemblers.Criterion
{
    public class JunctionParameterAssembler : ParameterAssembler
    {
        private IParameterBuilderFactory _builder;
        public JunctionParameterAssembler(IParameterBuilderFactory builder, AssemblyLine<IParameter, ParameterDTO> successor = null)
            : base(successor)
        {
            _builder = builder;
        }

        public override dbqf.Criterion.IParameter Restore(DTO.Criterion.ParameterDTO dto)
        {
            var j = dto as JunctionDTO;
            if (j == null)
                return base.Restore(dto);

            IJunction result = j is ConjunctionDTO ? _builder.Conjunction() : _builder.Disjunction();
            foreach (var p in j.Parameters)
                result.Add(Chain.Restore(p));

            return result;
        }

        public override DTO.Criterion.ParameterDTO Create(dbqf.Criterion.IParameter p)
        {
            var j = p as Junction;
            if (j == null)
                return base.Create(p);

            JunctionDTO result = j is Conjunction ? (JunctionDTO)new ConjunctionDTO() : (JunctionDTO)new DisjunctionDTO();
            result.Parameters = new List<ParameterDTO>();
            foreach (var p2 in j)
                result.Parameters.Add(Chain.Create(p2));

            return result;
        }
    }
}
