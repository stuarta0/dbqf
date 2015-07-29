using System;

namespace dbqf.Criterion.Builders
{
    public class BooleanBuilder : ParameterBuilder
    {
        public virtual bool Value
        {
            get { return _value; }
            set
            {
                _value = value;
                ComputeHash();
            }
        }
        private bool _value;

        public override string Label
        {
            get
            {
                if (base.Label == null)
                    return String.Concat("is ", Value).ToLower();
                return base.Label;
            }
            set { base.Label = value; }
        }

        public BooleanBuilder()
        {
        }
        public BooleanBuilder(bool value)
        {
            Value = value;
        }

        /// <summary>
        /// Ignores values and just creates SimpleParameters that do or don't equal Value.
        /// </summary>
        public override IParameter Build(IFieldPath path, params object[] values)
        {
            return new SimpleParameter(path, (Value ? "<>" : "="), 0);
        }

        public override bool Equals(object obj)
        {
            if (obj is BooleanBuilder)
            {
                var other = (BooleanBuilder)obj;
                return base.Eq(this.Label, other.Label)
                    && base.Eq(this.Value, other.Value);
            }
            return base.Equals(obj);
        }

        protected override void ComputeHash()
        {
            base.ComputeHash();
            unchecked
            {
                _hash = (_hash * 7) + Value.GetHashCode();
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
