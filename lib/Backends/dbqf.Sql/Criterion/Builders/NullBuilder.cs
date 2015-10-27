using dbqf.Criterion;
using dbqf.Criterion.Builders;

namespace dbqf.Sql.Criterion.Builders
{
    public class NullBuilder : ParameterBuilder, INullBuilder
    {
        public NullBuilder()
        {
            Label = "is null";
        }

        /// <summary>
        /// Ignores values, just provides a NullParameter.
        /// </summary>
        public override IParameter Build(IFieldPath path, params object[] values)
        {
            return new SqlNullParameter(path);
        }

        public override bool Equals(object obj)
        {
            if (obj is NullBuilder)
            {
                var other = (NullBuilder)obj;
                return base.Eq(this.Label, other.Label);
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
