using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using Molto.Abstractions;

namespace Molto
{
    public class InMemoryDbConnectionProvider : IDbConnectionProvider
    {
        private readonly IDictionary<string, IDbConnectionMaker> _dbConnectionFactory = new Dictionary<string, IDbConnectionMaker>();

        public bool AddConnectionFactory(string name, IDbConnectionMaker maker )
        {
            if (_dbConnectionFactory.ContainsKey(name))
            {
                return false;
            }

            _dbConnectionFactory.Add(name, maker);
            return true;
        }


        public IDbConnection GetConnection(string name = null)
        {
            if (!_dbConnectionFactory.ContainsKey(name))
            {
                throw new Exception($"{name} connection not found");
            }

            return _dbConnectionFactory[name].NewConnection();
        }
    }
}