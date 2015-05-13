using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Criterion;
using dbqf.Display;
using ProtoBuf;
using System.Xml.Serialization;

namespace Standalone.Serialization.DTO.Criterion
{
    public class BetweenValueDTO
    {
        [XmlElement]
        public object From { get; set; }
        public bool ShouldSerializeFrom() { return From != null; }

        [XmlElement]
        public object To { get; set; }
        public bool ShouldSerializeTo() { return To != null; }

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
            return new BetweenValue(dto.From, dto.To);
        }
    }
}
