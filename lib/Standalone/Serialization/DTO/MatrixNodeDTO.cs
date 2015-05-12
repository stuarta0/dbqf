using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Serialization.DTO
{
    [ProtoContract]
    public class MatrixNodeDTO
    {
        [ProtoMember(1)]
        public string Query { get; set; }

        [ProtoMember(2)]
        public string ToolTip { get; set; }
    }
}
