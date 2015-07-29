using dbqf.Criterion.Values;

namespace dbqf.Criterion.Builders
{
    public class DateBetweenBuilder : ParameterBuilder
    {
        public DateBetweenBuilder()
        {
            Label = "between";
        }

        /// <summary>
        /// Works with BetweenValues of DateValues.
        /// Only processes the first value.  Use JunctionBuilder to combine multiple.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public override IParameter Build(IFieldPath path, params object[] values)
        {
            if (values == null || values.Length == 0)
                return null;

            var v = values[0] as BetweenValue;
            if (v != null)
            {
                var from = ((BetweenValue)v).From as DateValue;
                var to = ((BetweenValue)v).To as DateValue;

                if (from == null && to == null)
                    return null;
                else if (from != null && to != null)
                    return new Conjunction()
                        .Parameter(new SimpleParameter(path, ">=", from.Lower))
                        .Parameter(new SimpleParameter(path, "<", to.Upper));
                else if (to == null)
                    return new SimpleParameter(path, ">=", from.Lower);
                else if (from == null)
                    return new SimpleParameter(path, "<", to.Upper);
            }

            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj is DateBetweenBuilder)
            {
                var other = (DateBetweenBuilder)obj;
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
