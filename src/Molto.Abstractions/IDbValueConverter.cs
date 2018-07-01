using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Molto.Abstractions
{
    public interface IDbValueConverter
    {
        void SetValue(IDbDataParameter parameter, object value);
    }

    public interface IEntityDatabaseMapProvider
    {
        void Scan<T>();
        void AddMap<T>(EntityMap map = null);
        EntityMap Get<T>();
    }

    public class EntityMap
    {
        public EntityMap()
        {
            //The dictionary key is case sensitive: for faster search the convention here is to have always the key in lowercase
            Properties = new Dictionary<string, EntityPropertyMap>(); 
        }


        /// <summary>
        /// Cached columns part
        /// </summary>
        public string ColumnsCache { get; set; }

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
        public IDictionary<string, EntityPropertyMap> Properties { get; set; }

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
                    _primaryKey = Properties.FirstOrDefault(x => x.Value.IsPrimaryKey).Value;
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
        public bool Ignore { get; set; }
    }
}