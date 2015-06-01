using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Display.Builders;
using Standalone.Core.Serialization.DTO.Builders;
using Standalone.Core.Serialization.DTO.Criterion;
using System.Reflection;

namespace Standalone.Core.Serialization.Assemblers.Builders
{
    public class LikeBuilderAssembler : BuilderAssembler
    {
        public LikeBuilderAssembler(BuilderAssembler successor)
            : base(successor)
        {
        }

        public override ParameterBuilder Restore(ParameterBuilderDTO dto)
        {
            var sb = dto as LikeBuilderDTO;
            if (sb == null)
                return base.Restore(dto);

            // ensure we can restore the MatchMode from the name of the static fields on MatchMode
            var modes = typeof(MatchMode).GetFields(BindingFlags.Public | BindingFlags.Static); 
            var mode = modes
                .Where(f => f.FieldType == typeof(MatchMode) && f.Name.Equals(sb.Mode))
                .First();

            if (mode == null)
            {
                throw new ArgumentException(String.Format("MatchMode {0} does not match an internal static field of the same name.  Valid values are {1}.",
                    sb.Mode, String.Join(", ", modes.Convert(f => f.Name))));
            }

            return new LikeBuilder()
            {
                Label = sb.Label,
                Mode = (MatchMode)mode.GetValue(null)
            };
        }

        public override ParameterBuilderDTO Create(ParameterBuilder b)
        {
            var sb = b as LikeBuilder;
            if (sb == null)
                return base.Create(b);

            // find the instance of the MatchMode in the static fields
            var modes = typeof(MatchMode).GetFields(BindingFlags.Public | BindingFlags.Static);
            var mode = modes
                .Where(f => f.FieldType == typeof(MatchMode) && f.GetValue(null) == sb.Mode)
                .First();

            if (mode == null)
                throw new ArgumentException("The provided MatchMode does not match an internal static field on MatchMode.");

            return new LikeBuilderDTO() 
            { 
                Label = sb.Label,
                Mode = mode.Name
            };
        }
    }
}
