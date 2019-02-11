using System.Data;
using System.Data.SQLite;
using Molto.Abstractions;

namespace Molto.SQLite
{
    public class SQLiteConnectionMaker : IDbConnectionMaker
    {
        private readonly string _connectionString;

        public SQLiteConnectionMaker(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection NewConnection()
        {
            return new SQLiteConnection(_connectionString);
        }
    }
}
