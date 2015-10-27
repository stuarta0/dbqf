using System;
using System.Reflection;
using System.Text;
using dbqf.Criterion;
using dbqf.Criterion.Builders;
using dbqf.Serialization.DTO.Builders;
using dbqf.Sql.Criterion.Builders;

namespace dbqf.Serialization.Assemblers.Builders
{
    public class LikeBuilderAssembler : BuilderAssembler
    {
        public LikeBuilderAssembler(BuilderAssembler successor = null)
            : base(successor)
        {
        }

        public override IParameterBuilder Restore(ParameterBuilderDTO dto)
        {
            var sb = dto as LikeBuilderDTO;
            if (sb == null)
                return base.Restore(dto);

            // ensure we can restore the MatchMode from the name of the static fields on MatchMode
            var modes = typeof(MatchMode).GetFields(BindingFlags.Public | BindingFlags.Static);
            var validNames = new StringBuilder();
            FieldInfo mode = null;
            foreach (var f in modes)
            {
                validNames.AppendFormat("{0}, ", f.Name);
                if (mode == null && f.FieldType == typeof(MatchMode) && f.Name.Equals(sb.Mode))
                    mode = f;
            }

            if (mode == null)
            {
                throw new ArgumentException(String.Format("MatchMode {0} does not match an internal static field of the same name.  Valid values are {1}.",
                    sb.Mode, validNames.Remove(validNames.Length - 2, 2)));
            }

            return new LikeBuilder()
            {
                Label = sb.Label,
                Mode = (MatchMode)mode.GetValue(null)
            };
        }

        public override ParameterBuilderDTO Create(IParameterBuilder b)
        {
            var sb = b as LikeBuilder;
            if (sb == null)
                return base.Create(b);

            // find the instance of the MatchMode in the static fields
            var modes = typeof(MatchMode).GetFields(BindingFlags.Public | BindingFlags.Static);
            FieldInfo mode = null;
            foreach (var f in modes)
            {
                if (f.FieldType == typeof(MatchMode) && f.GetValue(null) == sb.Mode)
                {
                    mode = f;
                    break;
                }
            }

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
