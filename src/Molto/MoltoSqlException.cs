using System;
using System.Data;

namespace Molto
{
    public class MoltoSqlException : Exception
    {
        private readonly IDbCommand _command;

        public MoltoSqlException(IDbCommand command, Exception ex = null) : base(
            $"Exception during Sql Execution: '{command?.CommandText}'.", ex)
        {
            _command = command;
        }
    }
}