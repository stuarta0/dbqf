
namespace dbqf.Criterion.Builders
{
    public class NullBuilder : ParameterBuilder
    {
        public NullBuilder()
        {
            Label = "is null";
        }

        /// <summary>
        /// Ignores values, just provides a NullParameter.
        /// </summary>
        public override IParameter Build(FieldPath path, params object[] values)
        {
            return new NullParameter(path);
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
    }
}
