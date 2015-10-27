using dbqf.Configuration;
using dbqf.Processing;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Criterion
{
    public class BetweenParameter : IParameter
    {
        protected IFieldPath _path;
        protected object _lo;
        protected object _hi;

        public BetweenParameter(IField field, object lo, object hi)
            : this(FieldPath.FromDefault(field), lo, hi)
        {
        }

        public BetweenParameter(IFieldPath path, object lo, object hi)
        {
            _path = path;
            _lo = lo;
            _hi = hi;
        }
    }
}
