using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Molto.Abstractions;

namespace Molto
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

    public class Db : IDb
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private readonly IDbValueConverter _dbValueConverter;
        private readonly IDataReaderToPoco _dataReaderToPoco;
        private readonly ISqlQueryBuilder _sqlQueryBuilder;
        public const string QueryParamPrefix = "@";

        public IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = _dbConnectionProvider.GetConnection();
                }

                if (_connection.State == ConnectionState.Broken)
                {
                    _connection.Close();
                }

                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }

                return _connection;
            }
        }

        public Db(IDbConnectionProvider dbConnectionProvider, IDbValueConverter dbValueConverter, IDataReaderToPoco dataReaderToPoco, ISqlQueryBuilder sqlQueryBuilder)
        {
            _dbConnectionProvider = dbConnectionProvider ?? throw new ArgumentNullException(nameof(dbConnectionProvider));
            this._dbValueConverter = dbValueConverter ?? throw new ArgumentNullException(nameof(dbValueConverter));
            _dataReaderToPoco = dataReaderToPoco;
            _sqlQueryBuilder = sqlQueryBuilder;
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                if (_connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                }
            }
        }

        protected IDbCommand CreateCommand(string sql, params object[] args)
        {
            IDbCommand cmd = Connection.CreateCommand();

            if (_transaction != null)
            {
                cmd.Transaction = _transaction;
            }

            cmd.CommandText = sql;

            foreach (var item in args)
            {
                AddParameter(cmd, item);
            }

            return cmd;
        }

        public int Execute(string sql, params object[] args)
        {
            using (IDbCommand cmd = CreateCommand(sql, args))
            {
                try
                {
                    var result = cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"The query $'{sql}' throw '{ex.Message}' ", ex);
                }

            }
        }

        private void AddParameter(IDbCommand cmd, object value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = QueryParamPrefix + cmd.Parameters.Count;
            if (value == null)
            {
                p.Value = DBNull.Value;
            }
            else
            {
                _dbValueConverter.SetValue(p, value);
            }
            cmd.Parameters.Add(p);

        }

        protected IDataReader GetReader(IDbCommand command)
        {
            string commandText = command.CommandText;
            try
            {
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new MoltoSqlException(commandText, ex);
            }
        }

        protected async Task<DbDataReader> GetReaderAsync(DbCommand command)
        {
            string commandText = command.CommandText;
            try
            {
                return await command.ExecuteReaderAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new MoltoSqlException(commandText, ex);
            }
        }

        public async Task<IList<T>> QueryAsync<T>(string sql, params object[] args)
        {
            sql = _sqlQueryBuilder.SelectSql<T>(sql);
            using (var cmd = (DbCommand)CreateCommand(sql, args))
            {
                using (var r = await GetReaderAsync(cmd).ConfigureAwait(false))
                {
                    var result = new List<T>();
                    while (true)
                    {
                        if (!await r.ReadAsync().ConfigureAwait(false))
                        {
                            return result;
                        }
                        var item = _dataReaderToPoco.Convert<T>(r);
                        result.Add(item);
                    }
                }
            }
        }

        public IEnumerable<T> Query<T>(string sql, params object[] args)
        {
            sql = _sqlQueryBuilder.SelectSql<T>(sql);
            using (IDbCommand cmd = CreateCommand(sql, args))
            {
                using (var r = GetReader(cmd))
                {

                    while (true)
                    {
                        if (!r.Read()) yield break;
                        var item = _dataReaderToPoco.Convert<T>(r);
                        yield return item;
                    }
                }
            }
        }

        public int Insert<T>(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            var sql = _sqlQueryBuilder.InsertSql<T>();
            object[] values = _sqlQueryBuilder.GetValues<T>(item);
            var result = Execute(sql, values);
            return result;
        }

        public int Update<T>(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            var sql = _sqlQueryBuilder.UpdateSql<T>();
            //Get all the properties value except for the primary key
            var values = _sqlQueryBuilder.GetColumnsValue<T>(item).Where(x => !x.Key.IsPrimaryKey).Select(x => x.Value).ToList();
            values.Add(_sqlQueryBuilder.GetPrimaryKeyValue(item));
            var result = Execute(sql, values.ToArray());
            return result;
        }

        public int Delete<T>(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            var sql = _sqlQueryBuilder.DeleteSql<T>();
            object[] values = new object[1] { _sqlQueryBuilder.GetPrimaryKeyValue(item) };
            var result = Execute(sql, values);
            return result;
        }

        public long Count<T>(string sql = null, params object[] args)
        {
            var countSql = _sqlQueryBuilder.CountSql<T>(sql);
            var result = Query<long>(countSql, args).FirstOrDefault();
            return result;
        }

        public Page<T> Page<T>(long page, long itemsPerPage, string sql = null, params object[] args)
        {
            if (page < 1)
            {
                throw new ArgumentException($"{nameof(page)} can't be less than 1 (it's 1 base).");
            }
            if (itemsPerPage < 1)
            {
                throw new ArgumentException($"{nameof(itemsPerPage)} can't be less than 1.");
            }

            var result = new Page<T>();

            result.TotalItems = Count<T>(sql, args);
            var selectSql = _sqlQueryBuilder.SelectSql<T>(sql);
            var pageSql = _sqlQueryBuilder.PageSql<T>(selectSql, page, itemsPerPage, result.TotalItems);
            result.Items = Query<T>(pageSql).ToList();
            result.CurrentPage = page;
            result.ItemsPerPage = itemsPerPage;
            result.TotalItems = Count<T>(sql, args);
            result.TotalPages = result.TotalItems / itemsPerPage;
            if ((result.TotalItems % itemsPerPage) != 0)
                result.TotalPages++;

            return result;
        }

        public T Single<T>(string sql = null, params object[] args)
        {
          
            sql = _sqlQueryBuilder.SelectSql<T>(sql);
            var selectSql = _sqlQueryBuilder.SingleSql(sql);
            using (IDbCommand cmd = CreateCommand(selectSql, args))
            {
                using (var r = GetReader(cmd))
                {
                    if (!r.Read()) return default(T);
                    var item = _dataReaderToPoco.Convert<T>(r);
                    return item;
                }
            }
        }

    }
}