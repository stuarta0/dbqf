using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Display.Builders
{
    public class DateGtBuilder : ParameterBuilder
    {
        public DateGtBuilder()
        {
            Label = ">";
        }

        /// <summary>
        /// Assumes the values will be an array of arrays.  i.e. each value is a 1-dimensional array with 2 elements.
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
                    Junction.Add(new SimpleParameter(path, ">=", date.Upper));
                }
            }

            return Junction;
        }
    }
}
