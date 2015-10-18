
using dbqf.Criterion;
namespace dbqf.Sql.Criterion.Builders
{
    public class LikeBuilder : ParameterBuilder
    {
        public virtual MatchMode Mode 
        {
            get { return _mode; }
            set
            {
                _mode = value;
                ComputeHash();
            }
        }
        private MatchMode _mode;
        
        public override string Label
        {
            get
            {
                if (base.Label == null && Mode != null)
                {
                    if (Mode == MatchMode.Anywhere)
                        return "contains";
                    else if (Mode == MatchMode.Start)
                        return "starts with";
                    else if (Mode == MatchMode.End)
                        return "ends with";
                    else if (Mode == MatchMode.Exact)
                        return "matches";
                    return Mode.ToString();
                }
                return base.Label;
            }
            set 
            { 
                base.Label = value;
            }
        }

        public LikeBuilder()
            : this(MatchMode.Anywhere)
        {
        }
        public LikeBuilder(MatchMode mode)
        {
            Mode = mode;
        }

        /// <summary>
        /// Works with values which are strings.
        /// Only processes the first value.  Use JunctionBuilder to combine multiple.
        /// </summary>
        public override IParameter Build(IFieldPath path, params object[] values)
        {
            if (values == null || values.Length == 0)
                return null;

            var v = values[0];
            if (v is string)
                return new LikeParameter(path, (string)v, Mode);
            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj is LikeBuilder)
            {
                var other = (LikeBuilder)obj;
                return base.Eq(this.Label, other.Label)
                    && base.Eq(this.Mode, other.Mode);
            }
            return base.Equals(obj);
        }

        protected override void ComputeHash()
        {
            base.ComputeHash();
            unchecked
            {
                _hash = (_hash * 7) + Mode.GetHashCode();
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
