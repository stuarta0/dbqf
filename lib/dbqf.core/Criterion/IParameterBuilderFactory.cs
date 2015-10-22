using dbqf.Criterion.Builders;
using System.Collections.Generic;

namespace dbqf.Criterion
{
    public interface IParameterBuilderFactory
    {
        IJunction Conjunction(params IParameter[] parameters);
        IJunction Disjunction(params IParameter[] parameters);

        IList<IParameterBuilder> Build(IFieldPath path);
        IParameterBuilder GetDefault(IFieldPath path);
    }
}
