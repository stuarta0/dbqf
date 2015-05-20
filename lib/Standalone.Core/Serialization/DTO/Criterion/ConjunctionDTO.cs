using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Criterion;
using ProtoBuf;

namespace Standalone.Core.Serialization.DTO.Criterion
{
    [ProtoContract]
    public class ConjunctionDTO
    {
        [ProtoMember(1)]
        public IList<IParameter> Parameters { get; set; }

        public static implicit operator ConjunctionDTO(Conjunction source)
        {
            if (source == null)
                return null;

            return new ConjunctionDTO() { Parameters = new List<IParameter>(source) };
        }

        public static implicit operator Conjunction(ConjunctionDTO dto)
        {
            var source = new Conjunction();
            foreach (var p in dto.Parameters)
                source.Add(p);
            return source;
        }
    }
}
