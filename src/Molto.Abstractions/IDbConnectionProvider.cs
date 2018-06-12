using System.Data;

namespace Molto.Abstractions
{
    public interface IDbConnectionProvider
    {
        IDbConnection GetConnection(string name = null);
    }
}