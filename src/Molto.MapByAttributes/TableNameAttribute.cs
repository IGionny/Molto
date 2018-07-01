using System;

namespace Molto.MapByAttributes
{

    /// <summary>
    /// Use this to change the name of the table
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute : Attribute
    {
        public TableNameAttribute(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentException($"{nameof(tableName)} cannot be null or empty");
            }
            TableName = tableName;
        }
        public string TableName { get; private set; }
    }
}
