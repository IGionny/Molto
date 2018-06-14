using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            sb.Append(GetFields<T>(map));
            sb.Append(" FROM ");
            sb.Append(EscapeTableName(map.Table));
            sb.Append(sql);
            return sb.ToString();
        }

        public string GetFields<T>(EntityMap map)
        {
            IList<string> columns = map.Properties.Select(x => x.ColumnName).ToList();

            return string.Join(",", columns);
        }

        public string EscapeTableName(string table)
        {
            return table;
        }

    }
}