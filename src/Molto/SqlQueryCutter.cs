using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Molto.Abstractions;

namespace Molto
{
    public class SqlQueryCutter : ISqlQueryCutter
    {

        protected int FindFirstSelect(string sql)
        {
            return sql.IndexOf(Sql.Select, StringComparison.InvariantCultureIgnoreCase);
        }

        protected int FindFirstFrom(string sql)
        {
            return sql.IndexOf(Sql.From, StringComparison.InvariantCultureIgnoreCase);
        }

        public string TrimSelectStart(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql)) return null;

            //Remove undesired spaces
            sql = sql.Trim();

            /*
             * Cases:
             *
             * 1) 1 only select ad beginning:
             * SELECT
             *  field1, field 2
             * FROM ..
             * WHERE ..
             *
             * 2) 2 select: one used as field
             * SELECT
             *   field1, (SELECT..)
             * FROM ..
             * WHERE ..
             *
             * 3) A select OVER the FROM
             * SELECT
             *  field1
             * FROM ..
             * WHERE
             *  (SELECT..)          
             *
             */


            var firstSelect = FindFirstSelect(sql);

            //The query start with SELECT
            if (firstSelect == 0)
            {
                sql = sql.Substring(Sql.Select.Length);
                //no other select
                if (FindFirstSelect(sql) == -1)
                {
                    sql = sql.Substring(FindFirstFrom(sql));
                    return sql;
                }

                firstSelect = FindFirstSelect(sql);
            }
            
            //There are more than one select!
            //The First from is before or after the first select?
            var firstFrom = FindFirstFrom(sql);
            if (firstFrom < firstSelect)
            {
                //The select is AFTER the from: meaning it is in a join or in the where clause
                sql = sql.Substring(FindFirstFrom(sql));
                return sql;
            }

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