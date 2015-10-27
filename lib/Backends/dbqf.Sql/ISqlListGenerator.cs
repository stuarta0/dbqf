using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Sql.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace dbqf.Sql
{
    public interface ISqlListGenerator
    {
        IFieldPath Path { get; set; }
        string IdColumn { get; set; }
        string ValueColumn { get; set; }
        string SortBy { get; set; }

        void Validate();
        
        /// <summary>
        /// Generate a command to use for determining list data for a given field path.
        /// </summary>
        /// <param name="dbCommandType"></param>
        /// <returns></returns>
        void UpdateCommand(IDbCommand cmd);
    }
}
