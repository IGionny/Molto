using System;

namespace Molto.MapByAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public string ColumnName { get; }
        public bool IsPrimary { get; }
        public bool Ignore { get; }

        public ColumnAttribute(string columnName = null, bool isPrimary = false, bool ignore = false)
        {
            ColumnName = columnName;
            IsPrimary = isPrimary;
            Ignore = ignore;
        }
       

    }
}