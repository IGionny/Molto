using System;
using Molto.Abstractions;

namespace Molto.PostgreSql
{
    public class PostgreSqlQueryBuilder : SqlQueryBuilder
    {
        public PostgreSqlQueryBuilder(IEntityDatabaseMapProvider entityDatabaseMapProvider,
            ISqlQueryCutter sqlQueryCutter) : base(entityDatabaseMapProvider, sqlQueryCutter)
        {
        }

        public override string PageSql<T>(string sql, long page, long itemsPerPage, long resultTotalItems)
        {
            if (!sql.ToUpper().Contains("ORDER BY"))
                throw new Exception("SQL Server 2012 Paging via OFFSET requires an ORDER BY statement.");

            var skip = (page - 1) * itemsPerPage;
            return sql + $" OFFSET {skip}  ROWS FETCH NEXT {itemsPerPage}  ROWS ONLY";
        }
    }
}