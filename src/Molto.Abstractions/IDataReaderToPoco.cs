using System.Collections.Generic;
using System.Data;

namespace Molto.Abstractions
{
    public interface IDataReaderToPoco
    {
        T Convert<T>(IDataReader reader);
    }


    public interface ISqlQueryBuilder
    {
        string SelectSql<T>(string sql);
        string InsertSql<T>();
        string UpdateSql<T>();
        string Delete<T>();
        //
        object[] GetValues<T>(T item);
        IDictionary<EntityPropertyMap, object> GetColumnsValue<T>(T item);
        object GetPrimaryKeyValue<T>(T item);
    }
}