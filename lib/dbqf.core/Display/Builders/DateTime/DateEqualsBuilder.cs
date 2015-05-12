using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Display.Builders
{
    public class DateEqualsBuilder : ParameterBuilder
    {
        public DateEqualsBuilder()
        {
            Label = "=";
        }

        /// <summary>
        /// Works with DateValues
        /// </summary>
        public override IParameter Build(FieldPath path, params object[] values)
        {
            if (values == null)
                return null;

            Junction.Clear();
            foreach (var v in values)
            {
                if (v is DateValue)
                {
                    var date = (DateValue)v;
                    Junction.Add(new Conjunction()
                        .Parameter(new SimpleParameter(path, ">=", date.Lower))
                        .Parameter(new SimpleParameter(path, "<", date.Upper)));
                }
            }

            return Junction;
        }
    }
}
