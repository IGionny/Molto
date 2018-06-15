using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        /// <summary>
        /// The database table name related to the Entity
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// The Entity's type
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// The list of Properties/Columns/Converter
        /// </summary>
        public IList<EntityPropertyMap> Properties { get; set; }

        public object CreateInstance()
        {
            var result = Activator.CreateInstance(EntityType);
            return result;
        }

        private EntityPropertyMap _primaryKey;
        public EntityPropertyMap PrimaryKey
        {
            get
            {
                if (_primaryKey == null)
                {
                    _primaryKey = Properties.FirstOrDefault(x => x.IsPrimaryKey);
                }

                return _primaryKey;
            }
        }
    }

    public class EntityPropertyMap
    {
        public bool IsPrimaryKey { get; set; }
        public PropertyInfo Property { get; set; }
        public string ColumnName { get; set; }
    }
}