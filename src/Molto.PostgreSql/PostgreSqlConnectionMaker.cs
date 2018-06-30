using System.Data;
using Molto.Abstractions;
using Npgsql;

namespace Molto.PostgreSql
{
    public class PostgreSqlConnectionMaker : IDbConnectionMaker
    {
        private readonly string _connectionString;

        public PostgreSqlConnectionMaker(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection NewConnection()
        {
            var conn = new NpgsqlConnection(_connectionString);
            return conn;
        }
    }
}