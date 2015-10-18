using dbqf.Criterion;
using dbqf.Criterion.Builders;

namespace dbqf.Sql.Criterion.Builders
{
    public class JunctionBuilder : ParameterBuilder, IJunctionBuilder
    {
        private JunctionType _type;
        public virtual JunctionType Type
        {
            get { return _type; }
            set 
            { 
                _type = value; 
                ComputeHash(); 
            }
        }

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
                if (Other != null && base.Label == null)
                    return Other.Label;
                return base.Label;
            }
            set { base.Label = value; }
        }

        public JunctionBuilder(JunctionType type)
        {
            Type = type;
        }
        public JunctionBuilder(JunctionType type, IParameterBuilder other)
            : this(type)
        {
            Other = other;
        }

        /// <summary>
        /// Works with any value that can be interpreted by the data provider being used.
        /// </summary>
        public override IParameter Build(IFieldPath path, params object[] values)
        {
            if (values == null)
                return Other.Build(path, null);

            Junction j = (Type == JunctionType.Conjunction ? (Junction)new SqlConjunction() : new SqlDisjunction());
            foreach (var v in values)
            {
                var p = Other.Build(path, v);
                if (p != null)
                    j.Add(p);
            }

            if (j.Count == 0)
                return null;
            else if (j.Count == 1)
                return j[0];
            return j;
        }

        public override bool Equals(object obj)
        {
            if (obj is JunctionBuilder)
            {
                var other = (JunctionBuilder)obj;
                return base.Eq(this.Type, other.Type)
                    && base.Eq(this.Other, other.Other)
                    && base.Eq(this.Label, other.Label);
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
                _hash = (_hash * 7) + Type.GetHashCode();
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
