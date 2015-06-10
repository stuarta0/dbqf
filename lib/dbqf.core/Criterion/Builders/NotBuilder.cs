using System;

namespace dbqf.Criterion.Builders
{
    public class NotBuilder : ParameterBuilder
    {
        public virtual ParameterBuilder Other { get; set; }

        public override string Label
        {
            get
            {
                if (base.Label == null && Other != null)
                    return String.Concat("not ", Other.Label);
                return base.Label;
            }
            set { base.Label = value; }
        }

        public NotBuilder()
        { 
        }
        public NotBuilder(ParameterBuilder other)
        {
            Other = other;
        }

        public override IParameter Build(FieldPath path, params object[] values)
        {
            var other = Other.Build(path, values);
            if (other == null)
                return null;

            return new NotParameter(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is NotBuilder)
            {
                var other = (NotBuilder)obj;
                return base.Eq(this.Label, other.Label)
                    && base.Eq(this.Other, other.Other);
            }
            return base.Equals(obj);
        }
    }
}
