using System;
using System.Collections.Generic;
using Molto.Abstractions;

namespace Molto
{
    public class EntityDatabaseMapProvider : IEntityDatabaseMapProvider
    {
        private readonly IDictionary<Type, EntityMap> _maps = new Dictionary<Type, EntityMap>();

        public void Scan<T>()
        {
            throw new NotImplementedException();
        }

        public void AddMap<T>()
        {
            _maps.Add(typeof(T), BuildMap<T>());
        }

        public EntityMap Get<T>()
        {
            if (!_maps.ContainsKey(typeof(T)))
            {
                return null;
            }
            return _maps[typeof(T)];
        }

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
                bool isPrimaryKey = string.Equals(prop.Name, "Id", StringComparison.InvariantCultureIgnoreCase);
                var columName = prop.Name;
                result.Properties.Add(columName.ToLower(), new EntityPropertyMap
                {
                    IsPrimaryKey = isPrimaryKey,
                    Property = prop,
                    ColumnName = columName
                });
            }

            return result;
        }
    }
}