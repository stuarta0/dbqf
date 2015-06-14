
namespace dbqf.Criterion.Builders
{
    public class JunctionBuilder : ParameterBuilder
    {
        public virtual JunctionType Type { get; set; }
        public virtual ParameterBuilder Other { get; set; }
        
        public override string Label
        {
            get
            {
                if (base.Label == null)
                    return Other.Label;
                return base.Label;
            }
            set { base.Label = value; }
        }

        public JunctionBuilder(JunctionType type)
        {
            Type = type;
        }
        public JunctionBuilder(JunctionType type, ParameterBuilder other)
            : this(type)
        {
            Other = other;
        }

        /// <summary>
        /// Works with any value that can be interpreted by the data provider being used.
        /// </summary>
        public override IParameter Build(FieldPath path, params object[] values)
        {
            if (values == null)
                return Other.Build(path, null);

            Junction j = (Type == JunctionType.Conjunction ? (Junction)new Conjunction() : new Disjunction());
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
    }
}
