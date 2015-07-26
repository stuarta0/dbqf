using System.Collections.Generic;
using dbqf.Criterion;
using dbqf.Criterion.Builders;

namespace dbqf.Display
{
    public interface IParameterBuilderFactory
    {
        IList<ParameterBuilder> Build(IFieldPath path);
        ParameterBuilder GetDefault(IFieldPath path);
    }
}
