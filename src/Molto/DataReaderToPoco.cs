using System;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
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
                //From Int32 to Int64

                if (typeof(T) == typeof(Int64) && reader[0] is int number)
                {
                    return (T)(object)(long)number;
                }

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
                var prop = map.Properties.SingleOrDefault(x => string.Equals(x.ColumnName, name, StringComparison.InvariantCultureIgnoreCase));
                if (prop == null)
                {
                    continue;
                }

                var value = reader[i];
                if (value is DBNull)
                {
                    prop.Property.SetValue(result, null);
                }
                else
                {
                    var convertedValue = ConvertValue(value);
                    prop.Property.SetValue(result, convertedValue);
                }

            }
            return result;
        }

        public object ConvertValue(object dbValue)
        {
            if (dbValue == null)
            {
                return null;
            }

            if (dbValue is DateTime datetime)
            {
                //Force UTC kind
                return new DateTime((datetime).Ticks, DateTimeKind.Utc);
            }

            return dbValue;
        }

    }
}