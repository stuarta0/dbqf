using dbqf.Configuration;
using System;
using System.Collections.Generic;

namespace dbqf.Sql.Configuration
{
    public interface IMatrixConfiguration : IConfiguration
    {
        MatrixNode this[ISubject from, ISubject to] { get; }
        IMatrixConfiguration MatrixSubject(ISubject subject);
        IMatrixConfiguration Matrix(ISubject from, ISubject to, string sql, string tooltip);
    }
}
