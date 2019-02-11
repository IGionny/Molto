using System;
using Molto.Abstractions;

namespace Molto
{
    public class DirectPropertyEntityMapper : IEntityMapper
    {
        public EntityMap BuildMap<T>()
        {
            var result = new EntityMap();
            var type = typeof(T);
            result.Table = type.Name; //Convention:  EntityName === TableName
            result.EntityType = type;
            foreach (var prop in type.GetProperties())
            {
                //Convention: the primary key name is always 'Id'
                //todo: add mapping logic customizable
                var isPrimaryKey = string.Equals(prop.Name, "Id", StringComparison.InvariantCultureIgnoreCase);
                var columnName = prop.Name;
                result.Properties.Add(columnName.ToLower(), new EntityPropertyMap
                {
                    IsPrimaryKey = isPrimaryKey,
                    Property = prop,
                    ColumnName = columnName,
                });
            }

            return result;
        }
    }
}