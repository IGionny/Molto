using System;
using System.Data;
using Molto.Abstractions;
using System.Data.SqlClient;

namespace Molto.MsSql2014
{
    public class MsSql2014ConnectionMaker : IDbConnectionMaker
    {
        private readonly string _connectionString;

        public MsSql2014ConnectionMaker(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection NewConnection()
        {
            SqlConnection conn =  new SqlConnection(_connectionString);
            return conn;
        }
    }
}
