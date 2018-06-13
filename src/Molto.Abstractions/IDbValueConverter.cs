﻿using System;
using System.Collections.Generic;
using System.Data;
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
        public string SqlSelect { get; set; }
        public IList<EntityPropertyMap> Properties { get; set; }
    }

    public class EntityPropertyMap
    {
        public PropertyInfo Property { get; set; }
        public string ColumnName { get; set; }
    }
}