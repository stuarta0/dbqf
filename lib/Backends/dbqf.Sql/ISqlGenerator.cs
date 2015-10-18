using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Processing;
using dbqf.Sql.Configuration;
using dbqf.Sql.Criterion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace dbqf.Sql
{
    public enum SortDirection
    {
        Ascending,
        Descending
    }

    public interface ISqlGenerator
    {
        bool AliasColumns { get; set; }

        SqlGenerator Alias(bool alias);
        SqlGenerator Column(IFieldPath path);
        SqlGenerator Column(IEnumerable<IFieldPath> path);
        SqlGenerator Target(ISqlSubject subject);
        SqlGenerator Where(ISqlParameter where);
        SqlGenerator OrderBy(IFieldPath path, SortDirection direction);
        SqlGenerator GroupBy(IFieldPath path);
        SqlGenerator ColumnGroupBy(IFieldPath path);
        SqlGenerator ColumnOrderBy(IFieldPath path, SortDirection direction);
        SqlGenerator ColumnOrderByGroupBy(IFieldPath path, SortDirection direction);

        void Validate();

        /// <summary>
        /// Generate a command using the current columns, target and parameters
        /// </summary>
        /// <param name="dbCommandType"></param>
        /// <returns></returns>
        void UpdateCommand(IDbCommand cmd);
    }
}
