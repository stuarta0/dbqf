using dbqf.Configuration;
using dbqf.Criterion;
using System;
using System.Collections.Generic;

namespace dbqf.Display
{
    public interface IFieldPathFactory
    {
        IList<IFieldPath> GetFields(ISubject subject);

        IList<IFieldPath> GetFields(IRelationField field);
    }
}
