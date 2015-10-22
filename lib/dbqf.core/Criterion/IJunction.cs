using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Criterion
{
    public interface IJunction : IParameter, IList<IParameter>
    {
    }
}
