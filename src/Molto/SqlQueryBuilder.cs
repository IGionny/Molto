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
            FromToWhereSql(sb, map, sql);

            return sb.ToString();
        }

        protected void FromToWhereSql(StringBuilder sb, EntityMap map, string sql)
        {
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

        public IDictionary<EntityPropertyMap, object> GetColumnsValue<T>(T item)
        {
            var map = _entityDatabaseMapProvider.Get<T>();
            var result = new Dictionary<EntityPropertyMap, object>(map.Properties.Count);
            foreach (var property in map.Properties)
            {
                var value = property.Property.GetValue(item);
                result.Add(property, value);
            }

            return result;
        }

        public object GetPrimaryKeyValue<T>(T item)
        {
            var map = _entityDatabaseMapProvider.Get<T>();

            if (map.PrimaryKey == null)
            {
                throw new Exception($"The map of '{typeof(T).Name}' does not provide a PrimaryKey");
            }

            var result = map.PrimaryKey.Property.GetValue(item);

            return result;
        }

        public string CountSql<T>(string sql)
        {
            if (!sql.Trim().StartsWith("SELECT", StringComparison.InvariantCultureIgnoreCase))
            {
                var map = _entityDatabaseMapProvider.Get<T>();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) ");
                FromToWhereSql(sb, map, sql);
                return sb.ToString();
            }

            //Remove fields from Select to Form
            //SELECT  name, eta, etc.. FROM table WHERE.. -> SELECT COUNT(*) FROM table WHERE..
            //SELECT  name, (SELECT 1) FROM table WHERE.. -> SELECT COUNT(*) FROM table WHERE..
            throw new NotImplementedException();
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

        public string UpdateSql<T>()
        {
            var map = _entityDatabaseMapProvider.Get<T>();

            StringBuilder sb = new StringBuilder();

            sb.Append("UPDATE ");
            sb.Append(EscapeTableName(map.Table));
            sb.Append(" SET ");

            IList<string> columns = new List<string>(map.Properties.Count -1);
            int i = 0;
            foreach (var propertyMap in map.Properties.Where(x => !x.IsPrimaryKey))
            {
                columns.Add(propertyMap.ColumnName + " = @" + i + " ");
                i++;
            }

            sb.Append(string.Join(",", columns));

            sb.Append(" WHERE " + map.PrimaryKey.ColumnName + " = @" + i);
            return sb.ToString();
        }

        public string Delete<T>()
        {
            var map = _entityDatabaseMapProvider.Get<T>();

            StringBuilder sb = new StringBuilder();

            sb.Append("DELETE FROM ");
            sb.Append(EscapeTableName(map.Table));
            sb.Append(" WHERE " + map.PrimaryKey.ColumnName + " = @0 ");
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