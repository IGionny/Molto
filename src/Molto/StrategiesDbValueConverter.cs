using System;
using System.Data;
using Molto.Abstractions;

namespace Molto
{
    public partial class StrategiesDbValueConverter : IDbValueConverter
    {
        public void SetValue(IDbDataParameter parameter, object value)
        {
            if (value == null)
            {
                parameter.Value = DBNull.Value;
                return;
            }

            if (value is bool boolV)
            {
                parameter.Value = boolV ? 1 : 0;
                return;
            }

            if (value is Guid)
            {
                parameter.Value = value;
                parameter.DbType = DbType.Guid;
                parameter.Size = 40;
                return;
            }
            if (value is string stringV)
            {
                parameter.Size = Math.Max(stringV.Length + 1, 4000); // Help query plan caching by using common size
                parameter.Value = value;
                return;
            }

            if (value is Enum enumV)
            {
                parameter.Value = Convert.ChangeType(enumV, enumV.GetTypeCode());
                return;
            }



        }
    }
}