using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Display.Builders
{
    public class LikeBuilder : ParameterBuilder
    {
        public virtual MatchMode Mode { get; set; }
        
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
            set { base.Label = value; }
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
        /// </summary>
        public override IParameter Build(FieldPath path, params object[] values)
        {
            if (values == null)
                return null;

            Junction.Clear();
            foreach (var v in values)
            {
                if (v is string)
                    Junction.Add(new LikeParameter(path, (string)v, Mode));
            }

            return Junction;
        }

        public override bool Equals(object obj)
        {
            if (obj is LikeBuilder)
            {
                var other = (LikeBuilder)obj;
                return base.Eq(this.Junction, other.Junction)
                    && base.Eq(this.Label, other.Label)
                    && base.Eq(this.Mode, other.Mode);
            }
            return base.Equals(obj);
        }
    }
}
