namespace Shared.Http.Abstraction
{
    public interface IUpdateHttp<in TKey>
    {
         Task UpdateAsync<TRequest>(TKey id, TRequest updatedItem);
    }
}