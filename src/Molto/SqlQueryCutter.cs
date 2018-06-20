using System;
using Molto.Abstractions;

namespace Molto
{
    public class SqlQueryCutter : ISqlQueryCutter
    {

        public string CutOutSelect(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql)) return null;
            //todo
            throw new NotImplementedException();
        }

        public string Fields(string sql)
        {
            throw new NotImplementedException();
        }

        public string From(string sql)
        {
            throw new NotImplementedException();
        }

        public string Conditions(string sql)
        {
            throw new NotImplementedException();
        }

        public string Orders(string sql)
        {
            throw new NotImplementedException();
        }
    }
}