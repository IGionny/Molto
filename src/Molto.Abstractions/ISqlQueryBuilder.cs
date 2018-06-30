using System.Collections.Generic;

namespace Molto.Abstractions
{
    public interface ISqlQueryBuilder
    {
        string SelectSql<T>(string sql);
        string InsertSql<T>();
        string UpdateSql<T>();
        string DeleteSql<T>();

        string SingleSql(string sql);

        //
        object[] GetValues<T>(T item);
        IDictionary<EntityPropertyMap, object> GetColumnsValue<T>(T item);
        object GetPrimaryKeyValue<T>(T item);
        string CountSql<T>(string sql);
        string PageSql<T>(string sql, long page, long itemsPerPage, long resultTotalItems);
    }
}