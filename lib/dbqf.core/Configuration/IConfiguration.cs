using System;
using System.Collections.Generic;
namespace dbqf.Configuration
{
    public interface IConfiguration : IList<ISubject>
    {
        ISubject this[string displayName] { get; }
        IConfiguration Subject(ISubject subject);
    }
}
