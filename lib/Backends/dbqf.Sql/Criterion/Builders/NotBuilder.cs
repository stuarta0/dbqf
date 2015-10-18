using dbqf.Criterion;
using dbqf.Criterion.Builders;
using System;

namespace dbqf.Sql.Criterion.Builders
{
    public class NotBuilder : ParameterBuilder, INotBuilder
    {
        private IParameterBuilder _other;
        public virtual IParameterBuilder Other
        {
            get { return _other; }
            set
            {
                _other = value;
                ComputeHash();
            }
        }

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

        public override IParameter Build(IFieldPath path, params object[] values)
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

        protected override void ComputeHash()
        {
            base.ComputeHash();
            unchecked
            {
                if (Other != null)
                    _hash = (_hash * 7) + Other.GetHashCode();
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
