using System.Data;

namespace Molto.Abstractions
{
    public interface IDataReaderToPoco
    {
        T Convert<T>(IDataReader reader);
    }
}