using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Display
{
    public class DateValue
    {
        public DateTime Lower { get; set; }
        public DateTime Upper { get; set; }

        public DateValue()
        {
        }
        public DateValue(DateTime lower, DateTime upper)
        {
            Lower = lower;
            Upper = upper;
        }
    }
}
