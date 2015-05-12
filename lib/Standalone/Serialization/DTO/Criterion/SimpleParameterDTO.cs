using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Criterion;
using ProtoBuf;

namespace Standalone.Serialization.DTO.Criterion
{
    [ProtoContract]
    public class SimpleParameterDTO
    {
        [ProtoMember(1)]
        public FieldPathDTO Path { get; set; }

        [ProtoMember(2)]
        public string Operator { get; set; }

        [ProtoMember(3)]
        public MessageParam Value { get; set; }

        public static implicit operator SimpleParameterDTO(SimpleParameter source)
        {
            if (source == null)
                return null;

            return new SimpleParameterDTO()
            {
                Path = (FieldPathDTO)source.Path,
                Operator = source.Operator,
                Value = MessageParam.CreateDynamic(source.Value)
            };
        }

        public static implicit operator SimpleParameter(SimpleParameterDTO dto)
        {
            return new SimpleParameter((FieldPath)dto.Path, dto.Operator, dto.Value.UntypedValue);
        }
    }
}
