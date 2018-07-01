using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Molto.Abstractions
{
    public interface IDb : IDisposable
    {
        int Execute(string sql, params object[] args);

        IEnumerable<T> Query<T>(string sql, params object[] args);

        int Insert<T>(T item);

        int Update<T>(T item);

        int Delete<T>(T item);

        long Count<T>(string sql = null, params object[] args);

        Page<T> Page<T>(long page, long itemsPerPage, string sql = null, params object[] args);

        T Single<T>(string sql = null, params object[] args);

        Task<IList<T>> QueryAsync<T>(string sql = null, params object[] args);
    }
}