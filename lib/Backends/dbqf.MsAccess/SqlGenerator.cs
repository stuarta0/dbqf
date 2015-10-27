using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Processing;
using dbqf.Sql;
using dbqf.Sql.Configuration;
using dbqf.Sql.Criterion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace dbqf.MsAccess
{
    public class SqlGenerator : dbqf.Sql.SqlGenerator
    {
        public SqlGenerator(IMatrixConfiguration configuration)
            : base(configuration) { }
        
        /// <summary>
        /// Generate a command using the current columns, target and parameters
        /// </summary>
        /// <param name="dbCommandType"></param>
        /// <returns></returns>
        public virtual void UpdateCommand(IDbCommand cmd)
        {
            Validate();

            // might be able to use the base class logic if GetFromClause is enough to make it work for Access.
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Returns a string "table1 join table2 on x = y join table3 on a = b"
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        protected override string GetFromClause(UniqueFieldPaths paths)
        {
            throw new NotImplementedException();
        }
    }
}
