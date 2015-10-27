using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Sql;
using dbqf.Sql.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace dbqf.MsAccess
{
    public class SqlListGenerator : dbqf.Sql.SqlListGenerator
    {
        public SqlListGenerator(IMatrixConfiguration configuration)
            : base(configuration) { }

        /// <summary>
        /// Generate a command to use for determining list data for a given field path.
        /// </summary>
        /// <param name="dbCommandType"></param>
        /// <returns></returns>
        public virtual void UpdateCommand(IDbCommand cmd)
        {
            Validate();

            // create join statement through join from index 0, through each field with an inner join
            // to the last field.  If the last field List source does not contain an ID field, this 
            // will simply return the query defined by the field.

            throw new NotImplementedException();
        }
    }
}
