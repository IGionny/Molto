using System;
using System.Linq;
using Molto.Abstractions;

namespace Molto.MapByAttributes
{
    public class MapByAttributeEntityMapper : IEntityMapper
    {
        public EntityMap BuildMap<T>()
        {
            var result = new EntityMap();
            var type = typeof(T);
            result.Table = type.Name; 

            var tableNameAttribute = type.GetCustomAttributes(typeof(TableNameAttribute), true)?.SingleOrDefault();
            if (tableNameAttribute != null)
            {
                result.Table = ((TableNameAttribute) tableNameAttribute).TableName;
            }

            result.EntityType = type;
            foreach (var prop in type.GetProperties())
            {
                //Convention: the primary key name is always 'Id'
                //todo: add mapping logic customizable
                var isPrimaryKey = string.Equals(prop.Name, "Id", StringComparison.InvariantCultureIgnoreCase);
                var columnName = prop.Name;
                var map = new EntityPropertyMap
                {
                    IsPrimaryKey = isPrimaryKey,
                    Property = prop,
                    ColumnName = columnName,
                };

                var columnAttribute = prop.GetCustomAttributes(typeof(ColumnAttribute), true)?.SingleOrDefault();

                if (columnAttribute != null)
                {
                    var columnAttr = (ColumnAttribute) columnAttribute;
                    if (columnAttr.Ignore)
                    {
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(columnAttr.ColumnName))
                    {
                        map.ColumnName = columnAttr.ColumnName;
                    }

                    if (columnAttr.IsPrimary)
                    {
                        map.IsPrimaryKey = true;
                    }

                }

                result.Properties.Add(map.ColumnName.ToLower(), map);
            }

            return result;
        }
    }
}