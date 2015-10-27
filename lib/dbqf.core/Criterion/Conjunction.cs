using dbqf.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Criterion
{
    /// <summary>
    /// Represents a combination of parameters that are intersected (AND).
    /// </summary>
    public abstract class Conjunction : Junction
    {
        protected override string Op
        {
            get { return "and"; }
        }

        public Conjunction()
        {
        }
    }
}
