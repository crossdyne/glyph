namespace Shared.Http.Abstraction
{
    public interface IGetByIdHttp<TResponse, in TKey> where TResponse : class
    {
        Task<TResponse?> GetByIdAsync(TKey id);
    }
}