using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Molto.Abstractions
{
    public interface IDbValueConverter
    {
        void SetValue(IDbDataParameter parameter, object value);
    }

    public interface IEntityDatabaseMapProvider
    {
        void Scan<T>();
        void AddMap<T>();
        EntityMap Get<T>();
    }

    public class EntityMap
    {
        public EntityMap()
        {
            Properties = new List<EntityPropertyMap>();
        }
        public string Table { get; set; }
        public Type EntityType { get; set; }

       public IList<EntityPropertyMap> Properties { get; set; }

        public object CreateInstance()
        {
            var result = Activator.CreateInstance(EntityType);
            return result;
        }
    }

    public class EntityPropertyMap
    {
        public PropertyInfo Property { get; set; }
        public string ColumnName { get; set; }
    }
}