using dbqf.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Criterion
{
    /// <summary>
    /// Replacement for SearchCombination AND.
    /// </summary>
    public class Conjunction : Junction
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
