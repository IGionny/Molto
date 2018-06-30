using System;
using Molto.Abstractions;

namespace Molto.SQLite
{
    public class SQLiteSqlQueryBuilder : SqlQueryBuilder
    {
        public SQLiteSqlQueryBuilder(IEntityDatabaseMapProvider entityDatabaseMapProvider,
            ISqlQueryCutter sqlQueryCutter) : base(entityDatabaseMapProvider, sqlQueryCutter)
        {
        }

        public override string PageSql<T>(string sql, long page, long itemsPerPage, long resultTotalItems)
        {
            var skip = (page - 1) * itemsPerPage;

            sql = sql + $" LIMIT {itemsPerPage} OFFSET {skip} ";

            return sql;
        }
    }
}