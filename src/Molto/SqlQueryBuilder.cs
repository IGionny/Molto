using System;
using System.Text;
using Molto.Abstractions;

namespace Molto
{
    public class SqlQueryBuilder : ISqlQueryBuilder
    {
        private readonly IEntityDatabaseMapProvider _entityDatabaseMapProvider;

        public SqlQueryBuilder(IEntityDatabaseMapProvider entityDatabaseMapProvider)
        {
            _entityDatabaseMapProvider = entityDatabaseMapProvider ?? throw new ArgumentNullException(nameof(entityDatabaseMapProvider));
        }

        public string SelectSql<T>(string sql)
        {
            var map = _entityDatabaseMapProvider.Get<T>();

            if (sql.Trim().StartsWith("SELECT", StringComparison.InvariantCultureIgnoreCase))
            {
                return sql;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append(map.SqlSelect);
            sb.Append("FROM ");
            sb.Append(EscapeTableName(map.Table));
            sb.Append(sql);
            return sb.ToString();
        }

        public string EscapeTableName(string table)
        {
            return table;
        }

    }
}