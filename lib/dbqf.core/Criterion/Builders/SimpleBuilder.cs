
namespace dbqf.Criterion.Builders
{
    public class SimpleBuilder : ParameterBuilder
    {
        public virtual string Operator { get; set; }

        public override string Label
        {
            get
            {
                if (base.Label == null)
                    return Operator;
                return base.Label;
            }
            set { base.Label = value; }
        }

        public SimpleBuilder()
        {
        }
        public SimpleBuilder(string op)
        {
            Operator = op;
        }

        /// <summary>
        /// Works with any value that can be interpreted by the data provider being used.
        /// Only processes the first value.  Use JunctionBuilder to combine multiple.
        /// </summary>
        public override IParameter Build(FieldPath path, params object[] values)
        {
            if (values == null || values.Length == 0)
                return null;

            return new SimpleParameter(path, Operator, values[0]);
        }

        public override bool Equals(object obj)
        {
            if (obj is SimpleBuilder)
            {
                var other = (SimpleBuilder)obj;
                return base.Eq(this.Label, other.Label)
                    && base.Eq(this.Operator, other.Operator);
            }
            return base.Equals(obj);
        }
    }
}
