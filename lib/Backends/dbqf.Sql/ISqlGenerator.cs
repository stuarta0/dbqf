using dbqf.Criterion;
using dbqf.Sql.Configuration;
using dbqf.Sql.Criterion;
using System.Collections.Generic;
using System.Data;

namespace dbqf.Sql
{
    public enum SortDirection
    {
        Ascending,
        Descending
    }

    public interface ISqlGenerator
    {
        /// <summary>
        /// Adds a number of columns to retrieve.
        /// </summary>
        SqlGenerator Column(IEnumerable<IFieldPath> path);

        /// <summary>
        /// Sets the core target that will be queried.
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        SqlGenerator Target(ISqlSubject subject);

        /// <summary>
        /// Sets the parameters to limit results.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        SqlGenerator Where(ISqlParameter where);

        /// <summary>
        /// Ensure the structure of this generator is valid.
        /// </summary>
        void Validate();

        /// <summary>
        /// Generate a command using the current columns, target and parameters
        /// </summary>
        /// <param name="dbCommandType"></param>
        /// <returns></returns>
        void UpdateCommand(IDbCommand cmd);
    }
}
