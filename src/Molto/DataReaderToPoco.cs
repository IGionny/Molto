using System;
using System.Data;
using System.Linq;
using Molto.Abstractions;
using Molto.Utilities;

namespace Molto
{

    public class DataReaderToPoco : IDataReaderToPoco
    {
        private readonly IEntityDatabaseMapProvider _entityDatabaseMapProvider;

        public DataReaderToPoco(IEntityDatabaseMapProvider entityDatabaseMapProvider)
        {
            _entityDatabaseMapProvider = entityDatabaseMapProvider ?? throw new ArgumentNullException(nameof(entityDatabaseMapProvider));
        }

        public T Convert<T>(IDataReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            //if is a primitive like 
            if (TypeUtils.IsPrimitive<T>())
            {
                return (T)reader[0];
            }

            var map = _entityDatabaseMapProvider.Get<T>();
            if (map == null)
            {
                return default(T);
            }

            var result = (T)map.CreateInstance();

            for (var i = 0; i < reader.FieldCount; i++)
            {
                var name = reader.GetName(i);
                var prop = map.Properties.SingleOrDefault(x => x.ColumnName == name);
                if (prop == null)
                {
                    continue;
                }

                var value = reader[i];
                prop.Property.SetValue(result, value);

            }
            return result;
        }

    }
}