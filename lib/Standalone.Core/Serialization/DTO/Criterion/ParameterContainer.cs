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
    public class ParameterContainer
    {
        /// <summary>
        /// Gets or sets the project path that produced this parameter.  
        /// TODO: change to better identification mechanism.
        /// </summary>
        [ProtoMember(1)]
        public string ProjectFile { get; set; }

        /// <summary>
        /// Gets or sets the parameter to limit the result set.
        /// </summary>
        [ProtoMember(2)]
        public IParameter Parameter { get; set; }

        /// <summary>
        /// Gets or sets the list of fields to use for custom output.
        /// </summary>
        [ProtoMember(3)]
        public List<FieldPathDTO> Outputs { get; set; }
    }
}
