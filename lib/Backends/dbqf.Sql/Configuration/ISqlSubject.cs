using System;
using System.Collections.Generic;
namespace dbqf.Configuration
{
    public interface ISqlSubject : ISubject
    {
        string Sql { get; set; }
    }
}
