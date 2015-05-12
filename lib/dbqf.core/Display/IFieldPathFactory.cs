using dbqf.Configuration;
using dbqf.Criterion;
using System;
using System.Collections.Generic;

namespace dbqf.Display
{
    public interface IFieldPathFactory
    {
        IList<FieldPath> GetFields(ISubject subject);
    }
}
