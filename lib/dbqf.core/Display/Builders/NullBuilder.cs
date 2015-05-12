using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Display.Builders
{
    public class NullBuilder : ParameterBuilder
    {
        public NullBuilder()
        {
            Label = "is null";
        }

        /// <summary>
        /// Ignores values, just provides a NullParameter
        /// </summary>
        public override IParameter Build(FieldPath path, params object[] values)
        {
            return new NullParameter(path);
        }
    }
}
