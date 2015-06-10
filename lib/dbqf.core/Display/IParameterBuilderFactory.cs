using System.Collections.Generic;
using dbqf.Criterion;
using dbqf.Criterion.Builders;

namespace dbqf.Display
{
    public interface IParameterBuilderFactory
    {
        IList<ParameterBuilder> Build(FieldPath path);
        ParameterBuilder GetDefault(FieldPath path);
    }
}
