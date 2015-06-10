using dbqf.Criterion.Values;

namespace dbqf.Criterion.Builders
{
    public class DateEqualsBuilder : ParameterBuilder
    {
        public DateEqualsBuilder()
        {
            Label = "=";
        }

        /// <summary>
        /// Works with DateValues.
        /// Only processes the first value.  Use JunctionBuilder to combine multiple.
        /// </summary>
        public override IParameter Build(FieldPath path, params object[] values)
        {
            if (values == null || values.Length == 0)
                return null;

            if (values[0] is DateValue)
            {
                var date = (DateValue)values[0];
                return new Conjunction()
                    .Parameter(new SimpleParameter(path, ">=", date.Lower))
                    .Parameter(new SimpleParameter(path, "<", date.Upper));
            }

            return null;
        }
    }
}
