using dbqf.Criterion;
using dbqf.Criterion.Values;

namespace dbqf.Sql.Criterion.Builders
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
        public override IParameter Build(IFieldPath path, params object[] values)
        {
            if (values == null || values.Length == 0)
                return null;

            if (values[0] is DateValue)
            {
                var date = (DateValue)values[0];
                return new SqlConjunction()
                    .Parameter(new SimpleParameter(path, ">=", date.Lower))
                    .Parameter(new SimpleParameter(path, "<", date.Upper));
            }

            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj is DateEqualsBuilder)
            {
                var other = (DateEqualsBuilder)obj;
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
