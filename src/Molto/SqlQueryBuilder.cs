using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Molto.Abstractions;

namespace Molto
{
    public class SqlQueryBuilder : ISqlQueryBuilder
    {
        private readonly IEntityDatabaseMapProvider _entityDatabaseMapProvider;
        private readonly ISqlQueryCutter _sqlQueryCutter;

        public SqlQueryBuilder(IEntityDatabaseMapProvider entityDatabaseMapProvider, ISqlQueryCutter sqlQueryCutter)
        {
            _entityDatabaseMapProvider = entityDatabaseMapProvider ??
                                         throw new ArgumentNullException(nameof(entityDatabaseMapProvider));
            _sqlQueryCutter = sqlQueryCutter ?? throw new ArgumentNullException(nameof(sqlQueryCutter));
        }

        public virtual string SelectSql<T>(string sql)
        {
            if (!string.IsNullOrWhiteSpace(sql) &&
                sql.Trim().StartsWith(Sql.Select, StringComparison.InvariantCultureIgnoreCase))
            {
                return sql;
            }

            var map = _entityDatabaseMapProvider.Get<T>();

            var sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append(GetFields<T>(map));
            FromToWhereSql(sb, map, sql);

            return sb.ToString();
        }

        protected virtual void FromToWhereSql(StringBuilder sb, EntityMap map, string sql)
        {
            sb.Append(" FROM ");
            sb.Append(EscapeTableName(map.Table));
            if (!string.IsNullOrWhiteSpace(sql))
            {
                if (sql.StartsWith(Sql.Where, StringComparison.InvariantCulture))
                {
                    sb.Append(" ");
                }
                else if (!sql.StartsWith(Sql.Order, StringComparison.InvariantCultureIgnoreCase))
                {
                    sb.Append(" WHERE ");
                }

                if (!sql.StartsWith(" "))
                {
                    sb.Append(" ");
                }

                sb.Append(sql);
            }
        }

        public virtual string InsertSql<T>()
        {
            var map = _entityDatabaseMapProvider.Get<T>();

            IList<string> columns = map.Properties.Keys.ToList();

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

        public virtual IDictionary<EntityPropertyMap, object> GetColumnsValue<T>(T item)
        {
            var map = _entityDatabaseMapProvider.Get<T>();
            var result = new Dictionary<EntityPropertyMap, object>(map.Properties.Count);
            foreach (var property in map.Properties)
            {
                var value = property.Value.Property.GetValue(item);
                result.Add(property.Value, value);
            }

            return result;
        }

        public virtual object GetPrimaryKeyValue<T>(T item)
        {
            var map = _entityDatabaseMapProvider.Get<T>();

            if (map.PrimaryKey == null)
            {
                throw new Exception($"The map of '{typeof(T).Name}' does not provide a PrimaryKey");
            }

            return map.PrimaryKey.Property.GetValue(item);
        }

        public virtual string CountSql<T>(string sql)
        {
            //remove "Order by" from the Count query
            if (!string.IsNullOrWhiteSpace(sql) &&
                sql.Trim().StartsWith(Sql.Order, StringComparison.InvariantCultureIgnoreCase))
            {
                sql = null;
            }

            if (string.IsNullOrWhiteSpace(sql) ||
                !sql.Trim().StartsWith("SELECT", StringComparison.InvariantCultureIgnoreCase))
            {
                var map = _entityDatabaseMapProvider.Get<T>();
                var sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) ");
                FromToWhereSql(sb, map, sql);
                return sb.ToString();
            }

            //Remove fields from Select to Form
            //SELECT  name, eta, etc.. FROM table WHERE.. -> SELECT COUNT(*) FROM table WHERE..
            //SELECT  name, (SELECT 1) FROM table WHERE.. -> SELECT COUNT(*) FROM table WHERE..
            var query = _sqlQueryCutter.TrimSelectStart(sql);
            return "SELECT COUNT(*) " + query;
        }

        public virtual string PageSql<T>(string sql, long page, long itemsPerPage, long resultTotalItems)
        {
            return $"{sql} LIMIT {itemsPerPage} OFFSET {((page - 1) * itemsPerPage)} ";
        }

        public virtual string SingleSql(string sql)
        {
            return sql + " LIMIT 1 ";
        }

        public virtual object[] GetValues<T>(T item)
        {
            var map = _entityDatabaseMapProvider.Get<T>();
            IList<object> result = new List<object>(map.Properties.Count);
            foreach (var property in map.Properties)
            {
                result.Add(GetPropertyValue(property.Value, item));
            }

            return result.ToArray();
        }

        protected virtual object GetPropertyValue<T>(EntityPropertyMap property, T item)
        {
            return property.Property.GetValue(item);
        }

        public virtual string UpdateSql<T>()
        {
            var map = _entityDatabaseMapProvider.Get<T>();

            var sb = new StringBuilder();

            sb.Append("UPDATE ");
            sb.Append(EscapeTableName(map.Table));
            sb.Append(" SET ");

            IList<string> columns = new List<string>(map.Properties.Count - 1);
            int i = 0;
            foreach (var propertyMap in map.Properties.Values.Where(x => !x.IsPrimaryKey))
            {
                columns.Add(propertyMap.ColumnName + " = @" + i + " ");
                i++;
            }

            sb.Append(string.Join(",", columns));

            sb.Append(" WHERE " + map.PrimaryKey.ColumnName + " = @" + i);
            return sb.ToString();
        }

        public virtual string DeleteSql<T>()
        {
            var map = _entityDatabaseMapProvider.Get<T>();

            var sb = new StringBuilder();

            sb.Append("DELETE FROM ");
            sb.Append(EscapeTableName(map.Table));
            sb.Append(" WHERE " + map.PrimaryKey.ColumnName + " = @0 ");
            return sb.ToString();
        }

        public virtual string GetFields<T>(EntityMap map)
        {
            if (!string.IsNullOrWhiteSpace(map.ColumnsCache))
            {
                return map.ColumnsCache;
            }

            IList<string> columns = map.Properties.Values.Where(x => !x.Ignore).Select(x => x.ColumnName).ToList();

            var selectColumns = string.Join(",", columns);
            map.ColumnsCache = selectColumns;

            return map.ColumnsCache;
        }

        public virtual string EscapeTableName(string table)
        {
            return table;
        }

        public virtual string[] ReservedWords => new string[0];
    }
}