using System.Data;

namespace Molto.Abstractions
{
    public interface IDbConnectionProvider
    {
        IDbConnection GetConnection(string name = null);
        bool AddConnectionFactory(string name, IDbConnectionMaker maker);
    }

    public interface IDbConnectionMaker
    {
        IDbConnection NewConnection();
    }
}