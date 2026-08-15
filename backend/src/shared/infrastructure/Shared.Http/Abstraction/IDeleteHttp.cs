namespace Shared.Http.Abstraction
{
    public interface IDeleteHttp<in TKey>
    {
        Task DeleteAsync(TKey id);
    }
}