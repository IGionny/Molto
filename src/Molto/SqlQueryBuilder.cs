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
            if (!string.IsNullOrWhiteSpace(sql))
            {
                if (sql.StartsWith("WHERE", StringComparison.InvariantCulture))
                {
                    sb.Append(" ");
                }
                else
                {
                    sb.Append(" WHERE ");
                }

                sb.Append(sql);
            }

            return sb.ToString();
        }

        public string InsertSql<T>()
        {
            var map = _entityDatabaseMapProvider.Get<T>();

            IList<string> columns = map.Properties.Select(x => x.ColumnName).ToList();

            StringBuilder sb = new StringBuilder();

            sb.Append("INSERT INTO ");
            sb.Append(EscapeTableName(map.Table));
            sb.Append(" (");
            sb.Append(string.Join(",", columns));
            sb.Append(") VALUES (");          
            int i = -1;
            sb.Append(string.Join(",", columns.Select(x =>
            {
                i++;
                return "@" + i;
            })));
            sb.Append(")");
            return sb.ToString();

        }

        public object[] GetValues<T>(T item)
        {
            var map = _entityDatabaseMapProvider.Get<T>();
            IList<object> result = new List<object>(map.Properties.Count);
            foreach (var property in map.Properties)
            {
                var value = property.Property.GetValue(item);
                result.Add(value);
            }

            return result.ToArray();
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