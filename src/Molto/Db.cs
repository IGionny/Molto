using System;
using System.Collections.Generic;
using System.Data;
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

        Task<Page<T>> PageAsync<T>(long page, long itemsPerPage, string sql, params object[] args);
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


        public IEnumerable<T> Query<T>(string sql, params object[] args)
        {
            sql = _sqlQueryBuilder.SelectSql<T>(sql);
            using (IDbCommand cmd = CreateCommand(sql, args))
            {
                using (IDataReader r = cmd.ExecuteReader())
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
            var sql =_sqlQueryBuilder.InsertSql<T>();
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
            var sql = _sqlQueryBuilder.Delete<T>();
            object[] values = new object[1] {_sqlQueryBuilder.GetPrimaryKeyValue(item)};
            var result = Execute(sql, values);
            return result;
        }

        public Task<Page<T>> PageAsync<T>(long page, long itemsPerPage, string sql, params object[] args)
        {
            if (page < 1)
            {
                throw new ArgumentException($"{nameof(page)} can't be less than 1 (it's 1 base).");
            }
            if (itemsPerPage < 1)
            {
                throw new ArgumentException($"{nameof(itemsPerPage)} can't be less than 1.");
            }

            long offset = (page - 1) * itemsPerPage;

            var countSql = _sqlQueryBuilder.CountSql<T>(sql);


            throw new NotImplementedException();
        }
    }
}