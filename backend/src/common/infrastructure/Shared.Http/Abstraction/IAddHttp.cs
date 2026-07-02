namespace Shared.Http.Abstraction
{
    public interface IAddHttp<TResponse> where TResponse : class
    {
        Task<TResponse?> AddAsync<TRequest>(TRequest newItem);
    }
}