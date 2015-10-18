using dbqf.Criterion.Builders;
using System.Collections.Generic;

namespace dbqf.Criterion
{
    public interface IParameterBuilderFactory
    {
        IList<IParameterBuilder> Build(IFieldPath path);
        IParameterBuilder GetDefault(IFieldPath path);
    }
}
