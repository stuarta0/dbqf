using dbqf.Configuration;

namespace dbqf.Sql.Configuration
{
    public interface ISqlSubject : ISubject
    {
        string Sql { get; set; }
    }
}
