using System.Collections.Generic;
using dbqf.Criterion;
using dbqf.Serialization.DTO.Criterion;

namespace dbqf.Serialization.Assemblers.Criterion
{
    public class JunctionParameterAssembler : ParameterAssembler
    {
        public JunctionParameterAssembler(AssemblyLine<IParameter, ParameterDTO> successor = null)
            : base(successor)
        {
        }

        public override dbqf.Criterion.IParameter Restore(DTO.Criterion.ParameterDTO dto)
        {
            var j = dto as JunctionDTO;
            if (j == null)
                return base.Restore(dto);

            Junction result = j is ConjunctionDTO ? (Junction)new Conjunction() : (Junction)new Disjunction();
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
