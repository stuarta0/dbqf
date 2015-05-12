using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Display.Builders
{
    public class DateBetweenBuilder : ParameterBuilder
    {
        public DateBetweenBuilder()
        {
            Label = "between";
        }

        /// <summary>
        /// Works with BetweenValues of DateValues.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public override IParameter Build(FieldPath path, params object[] values)
        {
            if (values == null)
                return null;

            Junction.Clear();

            // increment the values in pairs
            foreach (var v in values)
            {
                if (v is BetweenValue)
                {
                    var from = ((BetweenValue)v).From as DateValue;
                    var to = ((BetweenValue)v).To as DateValue;

                    if (from == null && to == null)
                        return null;
                    else if (from != null && to != null)
                        Junction.Add(new Conjunction()
                            .Parameter(new SimpleParameter(path, ">=", from.Lower))
                            .Parameter(new SimpleParameter(path, "<", to.Upper)));
                    else if (to == null)
                        Junction.Add(new SimpleParameter(path, ">=", from.Lower));
                    else if (from == null)
                        Junction.Add(new SimpleParameter(path, "<", to.Upper));
                }
            }

            return Junction;
        }
    }
}
