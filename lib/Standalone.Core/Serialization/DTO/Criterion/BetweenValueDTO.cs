using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Criterion;
using dbqf.Display;
using ProtoBuf;

namespace Standalone.Core.Serialization.DTO.Criterion
{
    [ProtoContract]
    public class BetweenValueDTO
    {
        [ProtoMember(1)]
        public MessageParam From { get; set; }

        [ProtoMember(2)]
        public MessageParam To { get; set; }

        public static implicit operator BetweenValueDTO(BetweenValue source)
        {
            if (source == null)
                return null;

            return new BetweenValueDTO()
            {
                From = MessageParam.CreateDynamic(source.From),
                To = MessageParam.CreateDynamic(source.To)
            };
        }

        public static implicit operator BetweenValue(BetweenValueDTO dto)
        {
            return new BetweenValue(dto.From.UntypedValue, dto.To.UntypedValue);
        }
    }
}
