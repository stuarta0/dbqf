using dbqf.Configuration;
using System;
using System.Collections.Generic;

namespace dbqf.Sql.Configuration
{
    public interface IMatrixConfiguration : IConfiguration
    {
        MatrixNode this[ISqlSubject from, ISqlSubject to] { get; }
    }
}
