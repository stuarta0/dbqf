using dbqf.Criterion;
using dbqf.Display.Builders;
using System;
using System.Collections.Generic;

namespace dbqf.Display
{
    public interface IParameterBuilderFactory
    {
        IList<ParameterBuilder> Build(FieldPath path);
        ParameterBuilder GetDefault(FieldPath path);
    }
}
