using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Standalone.Core.Serialization.DTO.Criterion;

namespace Standalone.Core.Serialization.DTO.Builders
{
    [XmlRoot("LikeBuilder")]
    public class LikeBuilderDTO : ParameterBuilderDTO
    {
        /// <summary>
        /// Any of: Anywhere, Start, End, Exact
        /// </summary>
        [XmlAttribute]
        public string Mode { get; set; }
    }
}
