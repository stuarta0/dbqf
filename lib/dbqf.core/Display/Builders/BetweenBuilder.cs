using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Display.Builders
{
    public class BetweenBuilder : ParameterBuilder
    {
        public BetweenBuilder()
        {
            Label = "between";
        }

        /// <summary>
        /// Works with values that are BetweenValue objects.
        /// </summary>
        public override IParameter Build(FieldPath path, params object[] values)
        {
            if (values == null)
                return null;

            Junction.Clear();
            foreach (var v in values)
            {
                if (v is BetweenValue)
                {
                    var from = ((BetweenValue)v).From;
                    var to = ((BetweenValue)v).To;

                    if (from != null && to != null)
                        Junction.Add(new BetweenParameter(path, from, to));
                    else if (to == null)
                        Junction.Add(new SimpleParameter(path, ">=", from));
                    else if (from == null)
                        Junction.Add(new SimpleParameter(path, "<=", to));
                }
            }

            return Junction;
        }

        public override bool Equals(object obj)
        {
            if (obj is BetweenBuilder)
            {
                var other = (BetweenBuilder)obj;
                return base.Eq(this.Junction, other.Junction)
                    && base.Eq(this.Label, other.Label);
            }
            return base.Equals(obj);
        }
    }
}
