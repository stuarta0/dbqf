using System;
using System.Collections.Generic;
namespace dbqf.Configuration
{
    public interface IConfiguration : IList<ISubject>
    {
        MatrixNode this[ISubject from, ISubject to] { get; }
        ISubject this[string displayName] { get; }

        IConfiguration Subject(ISubject subject);
        IConfiguration Matrix(ISubject from, ISubject to, string sql, string tooltip);
    }
}
