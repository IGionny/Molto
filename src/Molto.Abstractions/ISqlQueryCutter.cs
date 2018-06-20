namespace Molto.Abstractions
{
    public interface ISqlQueryCutter
    {
        string Fields(string sql);

        string From(string sql);

        string Conditions(string sql);

        string Orders(string sql);
    }
}