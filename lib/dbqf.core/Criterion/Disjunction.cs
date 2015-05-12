using dbqf.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Criterion
{
    /// <summary>
    /// Replacement for SearchCombination OR.
    /// </summary>
    public class Disjunction : Junction
    {
        protected override string Op
        {
            get { return "or"; }
        }

        public Disjunction()
        {
        }
    }
}
