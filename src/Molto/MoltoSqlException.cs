using System;

namespace Molto
{
    public class MoltoSqlException : Exception
    {
        public MoltoSqlException(string sql, Exception ex = null) : base(
            $"Exception during Sql Execution: '{sql}'.", ex)
        {
        }
    }
}