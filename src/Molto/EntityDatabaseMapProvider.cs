using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Molto.Abstractions;
using Molto.Utilities;

namespace Molto
{
    public class EntityDatabaseMapProvider : IEntityDatabaseMapProvider
    {
        private readonly ConcurrentDictionary<Type, EntityMap> _maps = new ConcurrentDictionary<Type, EntityMap>();
        private readonly IEntityMapper _entityMapper;

        public EntityDatabaseMapProvider(IEntityMapper entityMapper)
        {
            _entityMapper = entityMapper ?? throw new ArgumentNullException(nameof(entityMapper));
        }

        public void Scan<T>()
        {
            throw new NotImplementedException();
        }

        public void AddMap<T>(EntityMap map = null)
        {
            _maps.TryAdd(typeof(T), map ?? _entityMapper.BuildMap<T>());
        }

        public EntityMap Get<T>()
        {
            if (!_maps.ContainsKey(typeof(T)))
            {
                if (TypeUtils.IsPrimitive<T>())
                {
                    return null;
                }
                AddMap<T>();
            }
            if (_maps.TryGetValue(typeof(T), out var map))
            {
                return map;
            }
            return null;
        }

        
    }
}