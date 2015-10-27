
namespace dbqf.Criterion.Builders
{
    public interface INotBuilder : IParameterBuilder
    {
        IParameterBuilder Other { get; set; }
    }
}
