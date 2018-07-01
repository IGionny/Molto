namespace Molto.Abstractions
{
    public interface IEntityMapper
    {
        EntityMap BuildMap<T>();
    }
}