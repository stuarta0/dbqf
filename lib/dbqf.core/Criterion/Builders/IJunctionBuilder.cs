
namespace dbqf.Criterion.Builders
{
    public interface IJunctionBuilder : IParameterBuilder
    {
        JunctionType Type { get; set; }
        IParameterBuilder Other { get; set; }
    }
}
