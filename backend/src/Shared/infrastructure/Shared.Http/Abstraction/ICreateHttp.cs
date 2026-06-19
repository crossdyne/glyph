namespace Shared.Http.Abstraction
{
    public interface ICreateHttp<TResponse> where TResponse : class
    {
        Task CreateAsync<TRequest>(TRequest newItem);
    }
}