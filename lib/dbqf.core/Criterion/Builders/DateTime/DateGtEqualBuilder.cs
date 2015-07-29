using dbqf.Criterion.Values;

namespace dbqf.Criterion.Builders
{
    public class DateGtEqualBuilder : ParameterBuilder
    {
        public DateGtEqualBuilder()
        {
            Label = ">=";
        }

        /// <summary>
        /// Works with DateValues.
        /// Only processes the first value.  Use JunctionBuilder to combine multiple.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public override IParameter Build(IFieldPath path, params object[] values)
        {
            if (values == null || values.Length == 0)
                return null;

            if (values[0] is DateValue)
            {
                var date = (DateValue)values[0];
                return new SimpleParameter(path, ">=", date.Lower);
            }

            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj is DateGtEqualBuilder)
            {
                var other = (DateGtEqualBuilder)obj;
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
