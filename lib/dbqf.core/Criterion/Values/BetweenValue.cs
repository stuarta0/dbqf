using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Criterion.Values
{
    public class BetweenValue
    {
        public object From { get; set; }
        public object To { get; set; }

        public BetweenValue()
        {
        }
        public BetweenValue(object from, object to)
        {
            From = from;
            To = to;
        }
    }
}
